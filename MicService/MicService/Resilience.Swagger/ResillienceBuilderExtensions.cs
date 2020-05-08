﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Resilience.Swagger.SwaggerOptions;
using Resillience;
using Resillience.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Resilience.Swagger
{
    /// <summary>
    /// Swagger服务扩展类
    /// </summary>
    public static class ResillienceBuilderExtensions
    {
        public static ResillienceBuilder AddResillienceSwagger(this ResillienceBuilder builder, IConfiguration configuration = null)
        {
            configuration = (configuration ?? builder.Services.BuildServiceProvider().GetService<IConfiguration>());
            IServiceCollection services = builder.Services;
            SwaggerOption swaggerOption = configuration.GetSection("Resillience:Swagger").Get<SwaggerOption>();
            bool enabled = swaggerOption.Enabled; 
            if (!enabled)
            {
                return builder;
            }
            string title = swaggerOption.Title;
            int version = swaggerOption.Version;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"v{version}", new OpenApiInfo() { Title = title, Version = $"{version}" });

                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file);
                });
            });
            return builder;
        }
        public static IApplicationBuilder UseResillienceSwagger(this IApplicationBuilder app)
        {
            IConfiguration configuration = app.ApplicationServices.GetService<IConfiguration>();
            SwaggerOption swaggerOption = configuration.GetSection("Resillience:Swagger").Get<SwaggerOption>();
            bool enabled = swaggerOption.Enabled;
            if (!enabled)
            {
                return app;
            }
            app.UseSwagger().UseSwaggerUI(options =>
            {
                string title = swaggerOption.Title;
                int version = swaggerOption.Version;
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} V{version}");

                #region EF生成的sql显示在swagger页面
                //bool miniProfilerEnabled = swaggerOption.MiniProfiler;
                //if (miniProfilerEnabled)
                //{
                //    options.IndexStream = () => GetType().Assembly.GetManifestResourceStream("Resillience.Swagger.index.html");
                //} 
                #endregion
            });
           
            return app;
        }
    }
} 
