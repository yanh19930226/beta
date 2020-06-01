using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            #region ��ȡ
            services.AddControllers();
            #endregion

            services.AddResillience()
                    .AddSeriLog()
                    .AddResillienceSwagger()
                    .AddEventBus()
                    .AddHangfire();

            #region ��ȡ�����֤
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
            // ��֤��Ȩ
            services.AddAuthorization();
            // Http����
            services.AddHttpClient();
            #endregion

        }

        public override void SuppertContainer(ResillienceContainer container)
        {
            container.EnableZeus(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region ��װ

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

            #region ��֤��Ȩ
            // ��֤��Ȩ
            app.UseAuthentication();
            #endregion

            app.UseResillienceSwagger()
               .UseEventBus(eventBus =>
               {
                   eventBus.Subscribe<TestIntegrationEvent, DealIntegrationEventHandler>();
               })
               .UseHangfire();

            #region Todo
            //.UseHangfire(j=> {
            //    j.AddJob(() => Console.WriteLine("��ʱ�������1"));
            //    j.AddJob(() => Console.WriteLine("��ʱ�������2"));
            //}); 
            #endregion

            #region TetsHangfire
            //RecurringJob.AddOrUpdate("��ʱ�������", () => ExecuteAsync(), CronType.Minute()); 
            #endregion
        }

        #region TetsHangfire
        public void ExecuteAsync()
        {
            Console.WriteLine("��ʱ�������");
            
        }
        #endregion
    }
}
