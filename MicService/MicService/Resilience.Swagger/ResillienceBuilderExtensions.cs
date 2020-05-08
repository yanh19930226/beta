using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Resillience;
using Resillience.Exceptions;
using Resillience.Util;
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
            IConfigurationSection section = configuration.GetSection("Resillience:Swagger");
            IServiceCollection services = builder.Services;
            bool enabled = configuration["Resillience:Swagger:Enabled"].CastTo(false);
            if (!enabled)
            {
                return builder;
            }
            string url = configuration["Resillience:Swagger:Url"];
            if (string.IsNullOrEmpty(url))
            {
                throw new ResillienceException("配置文件中Swagger节点的Url不能为空");
            }

            string title = configuration["Resillience:Swagger:Title"];
            int version = configuration["Resillience:Swagger:Version"].CastTo(1);

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
        public static ResillienceBuilder UseResillienceSwagger(this ResillienceBuilder builder)
        {
            IConfiguration configuration = builder.App.ApplicationServices.GetService<IConfiguration>();
            bool enabled = configuration["OSharp:Swagger:Enabled"].CastTo(false);
            if (!enabled)
            {
                return builder;
            }

            builder.App.UseSwagger().UseSwaggerUI(options =>
            {
                string url = configuration["OSharp:Swagger:Url"];
                string title = configuration["OSharp:Swagger:Title"];
                int version = configuration["OSharp:Swagger:Version"].CastTo(1);
                options.SwaggerEndpoint(url, $"{title} V{version}");
                bool miniProfilerEnabled = configuration["OSharp:Swagger:MiniProfiler"].CastTo(false);
                if (miniProfilerEnabled)
                {
                    options.IndexStream = () => GetType().Assembly.GetManifestResourceStream("OSharp.Swagger.index.html");
                }
            });
           
            return builder;
        }
    }
} 
