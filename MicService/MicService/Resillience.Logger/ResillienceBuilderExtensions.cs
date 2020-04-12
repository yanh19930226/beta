using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Resillience.Logging;
using Serilog;
using Serilog.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Logger
{
	public static class ResillienceBuilderExtensions
	{
		public static ResillienceBuilder AddLogger(this ResillienceBuilder builder, IConfiguration configuration = null)
		{
			configuration = (configuration ?? builder.Services.BuildServiceProvider().GetService<IConfiguration>());
			IConfigurationSection section = configuration.GetSection("Resillience:Logger");
			IServiceCollection services = builder.Services;
			services.AddSingleton((Func<IServiceProvider, Serilog.ILogger>)delegate
			{
				Log.Logger = new LoggerConfiguration().ReadFrom.ConfigurationSection(section).Enrich.FromLogContext().CreateLogger();
				return Log.Logger;
			});
			services.AddSingleton<IResillienceLogger, ResillienceLogger>();
			services.AddSingleton(typeof(IResillienceLogger<>), typeof(ResillienceLogger<>));
			services.AddSingleton((Func<IServiceProvider, ILoggerFactory>)((IServiceProvider provider) => new SerilogLoggerFactory(provider.GetService<Serilog.ILogger>())));
			return builder;
		}
	}
}
