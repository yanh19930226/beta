using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Resilience.Swagger;
using Resilience.Zeus;
using Resilience.Zeus.Infra.Data.Context;
using Resillience;
using Resillience.EventBus.RabbitMQ;
using Resillience.Logger;
using ServiceB.Auth;
using ServiceB.IntegrationEventHandlers.Deals;
using ServiceB.IntegrationEvents.Tests;
using System;
using System.Text;

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
                    .AddEventBus();

            #region 抽取身份验证
            // 身份验证
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
                           ValidAudience = JWT.Domain,
                           ValidIssuer =JWT.Domain,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT.SecurityKey))
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
            container.EnableZeus(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region 封装
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            #region 认证授权
            // 认证授权
            app.UseAuthentication();
            app.UseAuthorization(); 
            #endregion

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
