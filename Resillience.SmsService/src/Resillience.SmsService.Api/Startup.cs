using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resilience.Swagger;
using Resilience.Zeus;
using Resillience.EventBus.RabbitMQ;

namespace Resillience.SmsService.Api
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
                    .AddResillienceSwagger()
                    .AddEventBus();
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #endregion

            app.UseResillienceSwagger()
               .UseEventBus(eventBus =>
               {
               });
        }
    }
}
