using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Resillience.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Logger
{
	public static class ResillienceBuilderExtensions
	{
		public static ResillienceBuilder AddSeriLog(this ResillienceBuilder builder, IConfiguration configuration = null)
		{
			configuration = (configuration ?? builder.Services.BuildServiceProvider().GetService<IConfiguration>());
			IServiceCollection services = builder.Services;
			//services.AddSingleton<IResillienceLogger, ResillienceLogger>();
		    services.AddSingleton(sp =>
		    {
		    	string logTemplete = "[{Timestamp:HH:mm:ss}][{Level}]{NewLine}Source:{SourceContext}{NewLine}Message:{Message}{NewLine}{Exception}{NewLine}";

				Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.MinimumLevel.Override("System", LogEventLevel.Information)
				.WriteTo.Debug(
					outputTemplate: logTemplete)
				.WriteTo.Console(new ElasticsearchJsonFormatter(), LogEventLevel.Verbose)
				//.WriteTo.Console(LogEventLevel.Information, logTemplete)
				.ReadFrom.Configuration(configuration, "Resillience:Logger:Serilog").CreateLogger();
		    	return Log.Logger;
		    });

			services.AddSingleton((Func<IServiceProvider, ILoggerFactory>)((IServiceProvider provider) => new Serilog.Extensions.Logging.SerilogLoggerFactory(provider.GetService<Serilog.ILogger>())));

			return builder;
		}

		#region Todo
		public static ResillienceBuilder AddNLog(this ResillienceBuilder builder, IConfiguration configuration = null)
		{

			return builder;
		}

		public static ResillienceBuilder AddLog4Net(this ResillienceBuilder builder, IConfiguration configuration = null)
		{
			return builder;
		} 
		#endregion
	}
}
