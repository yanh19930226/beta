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
			services.AddSingleton<IResillienceLogger, ResillienceLogger>();
			services.AddSingleton( provider => new Serilog.Extensions.Logging.SerilogLoggerFactory(provider.GetService<Serilog.ILogger>()));
			services.AddSingleton(sp=>
			{
				Log.Logger = new LoggerConfiguration().ReadFrom.ConfigurationSection(section).Enrich.FromLogContext().WriteTo.Console().CreateLogger();
				return Log.Logger;
			});
			return builder;
		}
	}
}
