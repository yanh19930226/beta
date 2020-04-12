using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Resillience.EventBus.Abstractions;
using Resillience.EventBus.Events;
using Resillience.Extensions;
using Resillience.Logging;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.EventBus.RabbitMQ.EventBusRabbitMQ
{
	public class EventBusRabbitMQ : IEventBus, IDisposable
	{
		private readonly string BROKER_NAME = "resillience_event_bus";

		private readonly IRabbitMQPersistentConnection _persistentConnection;

		private readonly IResillienceLogger<EventBusRabbitMQ> _logger;

		private readonly IEventBusSubscriptionsManager _subsManager;

		private readonly ILifetimeScope _autofac;

		private readonly string AUTOFAC_SCOPE_NAME = "resillience_event_bus";

		private readonly int _retryCount;

		private IModel _consumerChannel;

		private string _queueName;

		public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, IResillienceLogger<EventBusRabbitMQ> logger, ILifetimeScope autofac, IEventBusSubscriptionsManager subsManager, string queueName = null, int retryCount = 5, string exchangeName = "demon_event_bus")
		{
			BROKER_NAME = exchangeName;
			AUTOFAC_SCOPE_NAME = exchangeName;
			_persistentConnection = (persistentConnection ?? throw new ArgumentNullException("persistentConnection"));
			_logger = (logger ?? throw new ArgumentNullException("logger"));
			_subsManager = (subsManager ?? new InMemoryEventBusSubscriptionsManager());
			_queueName = queueName;
			_autofac = autofac;
			_retryCount = retryCount;
			_subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
			_logger.Information("EventBusRabbitMQ Init: " + exchangeName);
		}

		public void RunConsumer()
		{
			_consumerChannel = CreateConsumerChannel();
		}

		private void SubsManager_OnEventRemoved(object sender, string eventName)
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}
			using (IModel model = _persistentConnection.CreateModel())
			{
				model.QueueUnbind(_queueName, BROKER_NAME, eventName);
				if (_subsManager.IsEmpty)
				{
					_queueName = string.Empty;
					_consumerChannel.Close();
				}
			}
		}

		public void Publish(IntegrationEvent @event)
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}
			RetryPolicy retryPolicy = Policy.Handle<BrokerUnreachableException>().Or<SocketException>().WaitAndRetry(_retryCount, (Func<int, TimeSpan>)((int retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2.0, retryAttempt))), (Action<Exception, TimeSpan>)delegate (Exception ex, TimeSpan time)
			{
				_logger.Warning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
			});
			IModel channel = _persistentConnection.CreateModel();
			try
			{
				string eventName = @event.GetType().Name;
				channel.ExchangeDeclare(BROKER_NAME, "direct");
				string s = JsonConvert.SerializeObject(@event);
				byte[] body = Encoding.UTF8.GetBytes(s);
				retryPolicy.Execute(delegate
				{
					IBasicProperties basicProperties = channel.CreateBasicProperties();
					basicProperties.DeliveryMode = 2;
					channel.BasicPublish(BROKER_NAME, eventName, mandatory: true, basicProperties, body);
				});
			}
			finally
			{
				if (channel != null)
				{
					channel.Dispose();
				}
			}
		}

		public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
		{
			_logger.Information("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());
			DoInternalSubscription(eventName);
			_subsManager.AddDynamicSubscription<TH>(eventName);
		}

		public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
		{
			string eventKey = _subsManager.GetEventKey<T>();
			DoInternalSubscription(eventKey);
			_logger.Information("Subscribing to event {EventName} with {EventHandler}", eventKey, typeof(TH).GetGenericTypeName());
			_subsManager.AddSubscription<T, TH>();
		}

		private void DoInternalSubscription(string eventName)
		{
			if (!_subsManager.HasSubscriptionsForEvent(eventName))
			{
				if (!_persistentConnection.IsConnected)
				{
					_persistentConnection.TryConnect();
				}
				using (IModel model = _persistentConnection.CreateModel())
				{
					model.ExchangeDeclare(BROKER_NAME, "direct");
					model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, null);
					model.QueueBind(_queueName, BROKER_NAME, eventName);
				}
			}
		}

		public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
		{
			string eventKey = _subsManager.GetEventKey<T>();
			_logger.Information("Unsubscribing from event {EventName}", eventKey);
			_subsManager.RemoveSubscription<T, TH>();
		}

		public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
		{
			_subsManager.RemoveDynamicSubscription<TH>(eventName);
		}

		public void Dispose()
		{
			if (_consumerChannel != null)
			{
				_consumerChannel.Dispose();
			}
			_subsManager.Clear();
		}

		private IModel CreateConsumerChannel()
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}
			IModel channel = _persistentConnection.CreateModel();
			channel.ExchangeDeclare(BROKER_NAME, "direct");
			channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, null);
			EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);
			eventingBasicConsumer.Received += async delegate (object model, BasicDeliverEventArgs ea)
			{
				string routingKey = ea.RoutingKey;
				string @string = Encoding.UTF8.GetString(ea.Body);
				await ProcessEvent(routingKey, @string);
				channel.BasicAck(ea.DeliveryTag, multiple: false);
			};
			channel.BasicConsume(_queueName, autoAck: false, eventingBasicConsumer);
			channel.CallbackException += delegate
			{
				_consumerChannel.Dispose();
				_consumerChannel = CreateConsumerChannel();
			};
			return channel;
		}

		#region MyRegion
		//private unsafe async Task ProcessEvent(string eventName, string message)
		//{
		//	_logger.Information("eventName:" + eventName + ",message:" + message);
		//	if (_subsManager.HasSubscriptionsForEvent(eventName))
		//	{
		//		using (ILifetimeScope scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
		//		{
		//			IEnumerable<InMemoryEventBusSubscriptionsManager.SubscriptionInfo> handlersForEvent = _subsManager.GetHandlersForEvent(eventName);
		//			foreach (InMemoryEventBusSubscriptionsManager.SubscriptionInfo item in handlersForEvent)
		//			{
		//				if (item.IsDynamic)
		//				{
		//					IDynamicIntegrationEventHandler dynamicIntegrationEventHandler = scope.ResolveOptional(item.HandlerType) as IDynamicIntegrationEventHandler;
		//					if (dynamicIntegrationEventHandler != null)
		//					{
		//						dynamic val = JObject.Parse(message);
		//						dynamic val2 = dynamicIntegrationEventHandler.Handle(val).GetAwaiter();
		//						if (!(bool)val2.IsCompleted)
		//						{
		//							ICriticalNotifyCompletion awaiter = val2 as ICriticalNotifyCompletion;
		//							AsyncTaskMethodBuilder asyncTaskMethodBuilder = default(AsyncTaskMethodBuilder);
		//							if (awaiter == null)
		//							{
		//								INotifyCompletion awaiter2 = (INotifyCompletion)val2;
		//								asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter2, ref *(_003CProcessEvent_003Ed__20*)/*Error near IL_0246: stateMachine*/);
		//							}
		//							else
		//							{
		//								asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref *(_003CProcessEvent_003Ed__20*)/*Error near IL_0259: stateMachine*/);
		//							}
		//							/*Error near IL_0262: leave MoveNext - await not detected correctly*/;
		//						}
		//						val2.GetResult();
		//					}
		//				}
		//				else
		//				{
		//					object obj = scope.ResolveOptional(item.HandlerType);
		//					if (obj != null)
		//					{
		//						Type eventTypeByName = _subsManager.GetEventTypeByName(eventName);
		//						object obj2 = JsonConvert.DeserializeObject(message, eventTypeByName);
		//						await (Task)typeof(IIntegrationEventHandler<>).MakeGenericType(eventTypeByName).GetMethod("Handle")!.Invoke(obj, new object[1]
		//						{
		//							obj2
		//						});
		//					}
		//				}
		//			}
		//		}
		//	}
		//}

		#endregion

		private async Task ProcessEvent(string eventName, string message)
		{
			_logger.Information("eventName:" + eventName + ",message:" + message);

			if (_subsManager.HasSubscriptionsForEvent(eventName))
			{
				using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
				{
					var subscriptions = _subsManager.GetHandlersForEvent(eventName);
					foreach (var subscription in subscriptions)
					{
						if (subscription.IsDynamic)
						{
							var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
							if (handler == null) continue;
							dynamic eventData = JObject.Parse(message);

							await Task.Yield();
							await handler.Handle(eventData);
						}
						else
						{
							var handler = scope.ResolveOptional(subscription.HandlerType);
							if (handler == null) continue;
							var eventType = _subsManager.GetEventTypeByName(eventName);
							var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
							var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

							await Task.Yield();
							await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
						}
					}
				}
			}
			else
			{
				_logger.Information("No subscription for RabbitMQ event: {EventName}", eventName);
			}
		}

	}
}
