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
using Microsoft.Extensions.Options;
using Resilience.Swagger;
using Resilience.Zeus;
using Resillience.EventBus.RabbitMQ;
using Resillience.Logger;
using Resillience.SmsService.AliSms.SDK;
using Resillience.SmsService.Api.Infra.Services;
using Resillience.SmsService.TencentSms.SDK;

namespace Resillience.SmsService.Api
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

            #region DI
            services.Configure<Appsettings>(Configuration.GetSection("Appsettings"));
            var settings = services.BuildServiceProvider().GetService<IOptions<Appsettings>>().Value;

            //services.AddSingleton(new AliSmsClient(settings.Ali.AccessKeyId, settings.Ali.AccessSecret, ProtocolType.HTTP, Endpoint.Send));
            //services.AddSingleton(new TencentSmsClient(settings.Ali.AccessKeyId, settings.Ali.AccessSecret, ProtocolType.HTTP, Endpoint.Send)); 

            //services.AddSingleton<AliyunSmsService>();
            //services.AddSingleton<TencentSmsService>();
            //services.AddSingleton<SmsFactory>();

            #endregion

            services.AddResillience()
                     .AddSeriLog()
                    .AddResillienceSwagger()
                    .AddEventBus();
        }

        public override void SuppertContainer(ResillienceContainer container)
        {
            container.EnableZeus(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region ∑‚◊∞

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
