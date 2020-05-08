using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			string subscriptionClientName = ResillienceEventBusOptions.SubscriptionClientName;
			services.AddTransient<IIntegrationEventService, IntegrationEventService>();
			services.AddSingleton(sp=>
			{
				ConnectionFactory connectionFactory = new ConnectionFactory
				{
					HostName = ResillienceEventBusOptions.EventBusConnection
				};
				if (!string.IsNullOrEmpty(ResillienceEventBusOptions.EventBusUserName))
				{
					connectionFactory.UserName = ResillienceEventBusOptions.EventBusUserName;
				}
				if (!string.IsNullOrEmpty(ResillienceEventBusOptions.EventBusPassword))
				{
					connectionFactory.Password = ResillienceEventBusOptions.EventBusPassword;
				}
				int eventBusRetryCount = ResillienceEventBusOptions.EventBusRetryCount;
				return new DefaultRabbitMQPersistentConnection(connectionFactory, eventBusRetryCount);
			});
			services.AddSingleton(sp=>
			{
				IRabbitMQPersistentConnection requiredService = sp.GetRequiredService<IRabbitMQPersistentConnection>();
				IResillienceLogger<EventBusRabbitMQ.EventBusRabbitMQ> requiredService2 = sp.GetRequiredService<IResillienceLogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
				ILifetimeScope requiredService3 = sp.GetRequiredService<ILifetimeScope>();
				IEventBusSubscriptionsManager requiredService4 = sp.GetRequiredService<IEventBusSubscriptionsManager>();
				int eventBusRetryCount = ResillienceEventBusOptions.EventBusRetryCount;
				string exchangeName = ResillienceEventBusOptions.ExchangeName;
				return new EventBusRabbitMQ.EventBusRabbitMQ(requiredService, requiredService2, requiredService3, requiredService4, subscriptionClientName, eventBusRetryCount, exchangeName);
			});
			foreach (Type serviceType in Assembly.GetEntryAssembly().GetTypes().Where((Type t) => t.GetInterfaces().Contains(typeof(IIntegrationEventHandler))).ToArray<Type>())
			{
				services.AddTransient(serviceType);
			}
			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
		}

		public static IApplicationBuilder UseEventBus(this IApplicationBuilder app, Action<IEventBus> bindAction = null)
		{
			IEventBus requiredService = app.ApplicationServices.GetRequiredService<IEventBus>();
			if (bindAction != null)
			{
				bindAction(requiredService);
			}
			requiredService.RunConsumer();
			return app;
		}
	}
}
