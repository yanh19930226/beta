using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Resillience;
using Resillience.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 可配置扩展方法
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static ResillienceBuilder AddResillience(this IServiceCollection services, Action<ResillienceOption> setupAction)
        {
            if (setupAction == null)
            {
                setupAction = delegate
                {
                };
            }
            ResillienceOption obj = new ResillienceOption();
            setupAction?.Invoke(obj);
            return services.Configure(setupAction).AddResillienceCode();
        }
        /// <summary>
        /// 不可配置扩展方法
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ResillienceBuilder AddResillience(this IServiceCollection services, Configuration.IConfiguration configuration = null)
        {
            configuration = (configuration ?? services.BuildServiceProvider().GetService<Configuration.IConfiguration>());
            IConfigurationSection section = configuration.GetSection("Resillience");
            return services.Configure<ResillienceOption>(section).AddResillienceCode();
        }
        private static ResillienceBuilder AddResillienceCode(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddRouting();
            //services.AddControllers();

            return new ResillienceBuilder(services);
        }
    }
}
