using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resilience.Event.IntegrationEventLog.Services;
using Resillience;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Resilience.Event.IntegrationEventLog
{
    public static class ResillienceBuilderExtensions
    {
        public static ResillienceBuilder AddIntegrationEventLog(this ResillienceBuilder builder, string name,/*Func<Assembly> assembly,*/IConfiguration configuration = null)
        {
            configuration = (configuration ?? builder.Services.BuildServiceProvider().GetService<IConfiguration>());
            IServiceCollection services = builder.Services;
            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseMySql(configuration["Resillience:Zeus:Connection"],
                   mySqlOptionsAction: sqlOptions =>
                   {
                       sqlOptions.MigrationsAssembly(name);
                       sqlOptions.EnableRetryOnFailure(maxRetryCount: 10,
                           maxRetryDelay: TimeSpan.FromSeconds(30),
                           errorNumbersToAdd: null);
                   });
            });
            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();
            return builder;
        }
    }
}
