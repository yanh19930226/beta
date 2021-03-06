using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Resilience.Event.IntegrationEventLog;
using Resilience.Event.IntegrationEventLog.Services;
using Resilience.Swagger;
using Resilience.Zeus;
using Resilience.Zeus.Infra.Data.Context;
using Resillience;
using Resillience.EventBus.RabbitMQ;
using Resillience.Hangfire;
using Resillience.Logger;
using ServiceB.Auth;
using ServiceB.IntegrationEventHandlers.Deals;
using ServiceB.IntegrationEvents.Tests;
using ServiceB.Jobs;
using System;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceB
{
    public class Startup : CommonStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void SupportServices(IServiceCollection services)
        {
            #region 抽取
            services.AddControllers();
            #endregion

            services.AddResillience()
                         .AddSeriLog()
                         .AddResillienceSwagger()
                         .AddIntegrationEventLog(Assembly.GetExecutingAssembly().GetName().Name)
                         .AddEventBus();
            //.AddHangfire();

            #region 抽取身份验证
            services.Configure<Appsettings>(Configuration.GetSection("Appsettings"));
            var settings = services.BuildServiceProvider().GetService<IOptions<Appsettings>>().Value;


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ClockSkew = TimeSpan.FromSeconds(30),
                           ValidateIssuerSigningKey = true,
                           ValidAudience = settings.JWT.Domain,
                           ValidIssuer = settings.JWT.Domain,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWT.SecurityKey))
                       };
                   });


            // 认证授权
            services.AddAuthorization();
            // Http请求
            services.AddHttpClient();
            #endregion
        }

        public override void SuppertContainer(ResillienceContainer container)
        {
            container.EnableZeus(Assembly.GetExecutingAssembly().GetName().Name,Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseZeus()
                  .UseResillienceSwagger()
                  .UseEventBus(eventBus =>
                  {
                      eventBus.Subscribe<TestIntegrationEvent, DealIntegrationEventHandler>();
                  });
            //.UseHangfire(j =>
            //{
            //    j.AddJob(() => Console.WriteLine("定时任务测试1"));
            //    j.AddJob(() => Console.WriteLine("定时任务测试2"));
            //});
            #region 封装
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

            #region 认证授权
            // 认证授权
            app.UseAuthentication();
            #endregion
            
            #region Todo

            //var jobManager = app.ApplicationServices.GetRequiredService<IJobManager>();

            //var a=job.GetInvocationList();
            //foreach (var item in job.GetInvocationList())
            //{
            //    item.Method.Invoke()
            //}

            //RecurringJob.AddOrUpdate("定时任务测试1", () => Console.WriteLine("定时任务测试1"), Cron.Minutely());
            //RecurringJob.AddOrUpdate("定时任务测试2", () => Console.WriteLine("定时任务测试2"), Cron.Minutely());

            //jobManager.Run();

            #endregion
        }
    }
}
