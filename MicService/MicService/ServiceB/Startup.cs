using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Resilience.Swagger;
using Resilience.Zeus;
using Resilience.Zeus.Infra.Data.Context;
using Resillience;
using Resillience.EventBus;
using Resillience.EventBus.Abstractions;
using Resillience.EventBus.RabbitMQ;
using Resillience.EventBus.RabbitMQ.EventBusRabbitMQ;
using Resillience.EventBus.RabbitMQ.Options;
using Resillience.Logger;
using Resillience.Logging;
using ServiceB.IntegrationEventHandlers.Deals;
using ServiceB.IntegrationEvents.Tests;

namespace ServiceB
{
    public class Startup : CommonStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void SupportServices(IServiceCollection services)
        {
            #region ³éÈ¡
            services.AddControllers();
            #endregion

            services.AddResillience()
                    .AddSeriLog()
                    .AddResillienceSwagger();

            RegisterEventBus(services);
                    //.AddEventBus();
        }

        public override void SuppertContainer(ResillienceContainer container)
        {
            container.EnableZeus(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region ·â×°
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #endregion
            app.UseResillienceSwagger();

            ConfigureEventBus(app);
            //app.UseResillienceSwagger()
            //   .UseEventBus(eventBus =>
            //   {
            //       eventBus.Subscribe<TestIntegrationEvent, DealIntegrationEventHandler>();
            //   });
        }

        private void RegisterEventBus(IServiceCollection services)
        {

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["Resillience:EventBus:EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(Configuration["Resillience:EventBus:EventBusUserName"]))
                {
                    factory.UserName = Configuration["Resillience:EventBus:EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["Resillience:EventBus:EventBusPassword"]))
                {
                    factory.Password = Configuration["Resillience:EventBus:EventBusPassword"];
                }
                int eventBusRetryCount = 5;

                return new DefaultRabbitMQPersistentConnection(factory, eventBusRetryCount);
            });

            string subscriptionClientName = Configuration["Resillience:EventBus:SubscriptionClientName"];
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();

                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddTransient<DealIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TestIntegrationEvent, DealIntegrationEventHandler>();
        }
    }
}
