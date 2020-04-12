using Resillience.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Logger
{
	public class ResillienceLoggerFactory : IResillienceLoggerFactory
	{
		public readonly ILogger _logger;

		public ResillienceLoggerFactory(ILogger logger)
		{
			_logger = logger;
		}

		public IResillienceLogger CreateLogger()
		{
			return new ResillienceLogger(_logger);
		}
	}
}
