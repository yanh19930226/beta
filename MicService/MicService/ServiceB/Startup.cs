using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resilience.Swagger;
using Resilience.Zeus;
using Resilience.Zeus.Infra.Data.Context;
using Resillience;
using Resillience.EventBus.RabbitMQ;
using Resillience.Logger;
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
            #region ≥È»°
            services.AddControllers();
            #endregion

            #region code firsttest
            //services.AddDbContext<ZeusContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString("Connection"), b => b.MigrationsAssembly("Resilience.Zeus"));
            //}); 
            #endregion

            services
                .AddResillience()
                .AddSeriLog()
                .AddResillienceSwagger()
                .AddEventBus();
        }
        public override void SuppertContainer(ResillienceContainer container)
        {
            container.EnableZeus(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region ∑‚◊∞
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #endregion

            app.UseResillienceSwagger()
               .UseEventBus(eventBus =>
               {
                  eventBus.Subscribe<TestIntegrationEvent, DealIntegrationEventHandler>();
               });
        }

    }
}
