using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Resillience.EventBus.Abstractions;
using Resillience.EventBus.RabbitMQ.EventBusRabbitMQ;
using Resillience.EventBus.RabbitMQ.Options;
using Resillience.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Resillience.EventBus.RabbitMQ
{
	public static class ResillienceBuilderExtensions
	{
		public static ResillienceBuilder AddEventBus(this ResillienceBuilder builder, IConfiguration configuration = null)
		{
			configuration = (configuration ?? builder.Services.BuildServiceProvider().GetService<IConfiguration>());
			IConfigurationSection section = configuration.GetSection("Resillience:EventBus");
			builder.Services.Configure<ResillienceEventBusOptions>(section).AddEventBusCore();
			return builder;
		}

		private static void AddEventBusCore(this IServiceCollection services)
		{
			ServiceProvider provider = services.BuildServiceProvider();
			ResillienceEventBusOptions ResillienceEventBusOptions = provider.GetService<IOptions<ResillienceEventBusOptions>>().Value;


			//Create Connection
			services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
			{
				var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
				var factory = new ConnectionFactory()
				{
					HostName = ResillienceEventBusOptions.EventBusConnection,
					DispatchConsumersAsync = true
				};

				if (!string.IsNullOrEmpty(ResillienceEventBusOptions.EventBusUserName))
				{
					factory.UserName = ResillienceEventBusOptions.EventBusUserName;
				}

				if (!string.IsNullOrEmpty(ResillienceEventBusOptions.EventBusPassword))
				{
					factory.Password = ResillienceEventBusOptions.EventBusPassword;
				}
				int eventBusRetryCount = ResillienceEventBusOptions.EventBusRetryCount;

				return new DefaultRabbitMQPersistentConnection(factory, eventBusRetryCount);
			});

			string subscriptionClientName = ResillienceEventBusOptions.SubscriptionClientName;
			services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp =>
			{
				var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
				var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
				var logger = sp.GetRequiredService<ILogger< EventBusRabbitMQ.EventBusRabbitMQ>> ();

				var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

				var retryCount = ResillienceEventBusOptions.EventBusRetryCount;
				return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, ResillienceEventBusOptions.ExchangeName, subscriptionClientName, retryCount);
			});

			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

			//foreach IIntegrationEventHandler
			foreach (Type serviceType in Assembly.GetEntryAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIntegrationEventHandler))).ToArray())
			{
				services.AddTransient(serviceType);
			}
		}

		public static IApplicationBuilder UseEventBus(this IApplicationBuilder app, Action<IEventBus> bindAction = null)
		{
			var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
			if (bindAction != null)
			{
				bindAction(eventBus);
			}
			return app;
		}
	}
}
