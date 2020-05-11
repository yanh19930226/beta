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
using Resillience;
using Resillience.Logger;
using Resillience.Test;

namespace ServiceB
{
    public class Startup: CommonStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
        public override void SupportServices(IServiceCollection services)
        {
            #region ³éÈ¡
            services.AddControllers(); 
            #endregion

            services
                .AddResillience()
                .AddSeriLog()
                .AddResillienceSwagger()
                .AddTest();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region ·â×°
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

            app.UseResillienceSwagger();
        }
    }
}
