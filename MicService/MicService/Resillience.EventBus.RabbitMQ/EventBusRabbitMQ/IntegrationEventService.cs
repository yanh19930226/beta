using Resillience.EventBus.Abstractions;
using Resillience.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.EventBus.RabbitMQ.EventBusRabbitMQ
{
	public class IntegrationEventService : IIntegrationEventService
	{
		private readonly IEventBus _eventBus;

		public IntegrationEventService(IEventBus eventBus)
		{
			_eventBus = (eventBus ?? throw new ArgumentNullException("eventBus"));
		}

		public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
		{
			try
			{
				_eventBus.Publish(evt);
			}
			catch (Exception)
			{
			}
		}
	}
}
