using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
    //public static class SwaggerExtensions
    //{
    //    public static void AddResillienceSwagger(this IServiceCollection services)
    //    {
    //        IConfiguration configuration = services.GetConfiguration();
    //        bool enabled = configuration["Resillience:Swagger:Enabled"].CastTo(false);
    //        if (!enabled)
    //        {
    //            return services;
    //        }

    //        string url = configuration["Resillience:Swagger:Url"];
    //        if (string.IsNullOrEmpty(url))
    //        {
    //            throw new ResillienceException("配置文件中Swagger节点的Url不能为空");
    //        }

    //        string title = configuration["Resillience:Swagger:Title"];
    //        int version = configuration["Resillience:Swagger:Version"].CastTo(1);

    //        services.AddSwaggerGen(options =>
    //        {
    //            options.SwaggerDoc($"v{version}", new OpenApiInfo() { Title = title, Version = $"{version}" });

    //            Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
    //            {
    //                options.IncludeXmlComments(file);
    //            });
    //        });
    //        return services;
    //    }

    //    public static IApplicationBuilder UseResillienceSwagger(this IApplicationBuilder app)
    //    {
    //        return app;
    //    }
    //}
} 
