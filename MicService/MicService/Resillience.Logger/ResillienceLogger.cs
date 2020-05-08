using Resillience.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Logger
{
    public class ResillienceLogger : IResillienceLogger
    {
        public readonly ILogger _logger;

        public ResillienceLogger(ILogger logger)
        {
            _logger = logger;
        }
		public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this._logger.Debug(exception, messageTemplate, propertyValues);
		}

		public void Debug(string messageTemplate, params object[] propertyValues)
		{
			this._logger.Debug(messageTemplate, propertyValues);
		}

		public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this._logger.Error(exception, messageTemplate, propertyValues);
		}

		public void Error(string messageTemplate, params object[] propertyValues)
		{
			this._logger.Error(messageTemplate, propertyValues);
		}

		public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this._logger.Fatal(exception, messageTemplate, propertyValues);
		}

		public void Fatal(string messageTemplate, params object[] propertyValues)
		{
			this._logger.Fatal(messageTemplate, propertyValues);
		}

		public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this._logger.Information(exception, messageTemplate, propertyValues);
		}

		public void Information(string messageTemplate, params object[] propertyValues)
		{
			this._logger.Information(messageTemplate, propertyValues);
		}

		public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this._logger.Verbose(exception, messageTemplate, propertyValues);
		}

		public void Verbose(string messageTemplate, params object[] propertyValues)
		{
			this._logger.Verbose(messageTemplate, propertyValues);
		}

		public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this._logger.Warning(exception, messageTemplate, propertyValues);
		}

		public void Warning(string messageTemplate, params object[] propertyValues)
		{
			this._logger.Warning(messageTemplate, propertyValues);
		}
	}
    public class ResillienceLogger<TContext> : ResillienceLogger, IResillienceLogger<TContext>, IResillienceLogger
    {
        public ResillienceLogger(ILogger logger)
            : base(logger.ForContext<TContext>())
        {
        }
    }
}
