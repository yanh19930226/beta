using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Resillience.Logger.LoggerOptions;
using Resillience.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Logger
{
	public static class ResillienceBuilderExtensions
	{
        #region SeriLog
        public static ResillienceBuilder AddSeriLog(this ResillienceBuilder builder, IConfiguration configuration = null)
        {
            configuration = (configuration ?? builder.Services.BuildServiceProvider().GetService<IConfiguration>());
            LoggerOption loggerOption = configuration.GetSection("Resillience:Logger").Get<LoggerOption>();
            IServiceCollection services = builder.Services;

            //services.AddSingleton<IResillienceLogger, ResillienceLogger>();
            services.AddSingleton(sp =>
            {
                //控制台serilog日志展示模板
                string logTemplete = "[{Timestamp:HH:mm:ss}][{Level}]{NewLine}Source:{SourceContext}{NewLine}Message:{Message}{NewLine}{Exception}{NewLine}";
                //elasticsearch地址
                var EsUri = loggerOption.EsUri;
                //日志记录级别
                LogEventLevel logEventLevel = LogEventLevel.Verbose;
                LogLevel logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), loggerOption.Level);

                var LoggerConfiguration = new LoggerConfiguration();
                switch (logLevel)
                {
                    case LogLevel.Verbose:
                        LoggerConfiguration = LoggerConfiguration.MinimumLevel.Verbose();
                        logEventLevel = LogEventLevel.Verbose;
                        break;
                    case LogLevel.Debug:
                        LoggerConfiguration = LoggerConfiguration.MinimumLevel.Debug();
                        logEventLevel = LogEventLevel.Debug;
                        break;
                    case LogLevel.Information:
                        LoggerConfiguration = LoggerConfiguration.MinimumLevel.Information();
                        logEventLevel = LogEventLevel.Information;
                        break;
                    case LogLevel.Warning:
                        LoggerConfiguration = LoggerConfiguration.MinimumLevel.Warning();
                        logEventLevel = LogEventLevel.Warning;
                        break;
                    case LogLevel.Error:
                        LoggerConfiguration = LoggerConfiguration.MinimumLevel.Error();
                        logEventLevel = LogEventLevel.Error;
                        break;
                    case LogLevel.Fatal:
                        LoggerConfiguration = LoggerConfiguration.MinimumLevel.Fatal();
                        logEventLevel = LogEventLevel.Fatal;
                        break;
                    default:
                        LoggerConfiguration = LoggerConfiguration.MinimumLevel.Verbose();
                        logEventLevel = LogEventLevel.Verbose;
                        break;
                };

                LoggerConfiguration = LoggerConfiguration
                                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                                   .MinimumLevel.Override("System", LogEventLevel.Information);

                Log.Logger = LoggerConfiguration
                                     .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(EsUri))
                                     {
                                         AutoRegisterTemplate = true,
                                         FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                                         EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                                                                                          EmitEventFailureHandling.WriteToFailureSink |
                                                                                                          EmitEventFailureHandling.RaiseCallback,
                                         FailureSink = new FileSink("./Logs/log.txt", new JsonFormatter(), null)
                                     }).WriteTo.Console(logEventLevel, logTemplete).ReadFrom.Configuration(configuration, "Resillience:Logger:Serilog").CreateLogger();
                return Log.Logger;
            });

            services.AddSingleton((Func<IServiceProvider, ILoggerFactory>)((IServiceProvider provider) => new Serilog.Extensions.Logging.SerilogLoggerFactory(provider.GetService<Serilog.ILogger>())));

            return builder;
        } 
        #endregion

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
