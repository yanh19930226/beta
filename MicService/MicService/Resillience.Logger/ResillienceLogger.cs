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
            throw new NotImplementedException();
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            throw new NotImplementedException();
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
