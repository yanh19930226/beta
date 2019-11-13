using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Swashbuckle.AspNetCore.Swagger;

namespace Gateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //gateway配置认证授权
            var authenticationProviderKey = "beta";
            services.AddAuthentication()
               .AddIdentityServerAuthentication(authenticationProviderKey, options =>
               {
                   options.Authority = "http://localhost:5001";
                   options.ApiName = "gateway_api";
                   options.SupportedTokens = SupportedTokens.Both;
                   options.ApiSecret = "secret";
                   options.RequireHttpsMetadata = false;
               });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOcelot();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Configuration["Swagger:Name"],
                    new Info
                    {
                        Title = Configuration["Swagger:Title"],
                        Version = Configuration["Swagger:Version"]
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var apis = Configuration["Apis:SwaggerNames"].Split(";").ToList();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMvc();
                app.UseSwagger()
                   .UseSwaggerUI(options =>
                   {
                       apis.ToList().ForEach(key =>
                       {
                           options.SwaggerEndpoint($"/{key}/swagger.json", key);
                       });
                       options.DocumentTitle = "api网关";
                   });
            }
            app.UseOcelot().Wait();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
