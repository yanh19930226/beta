using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Logging
{
	public interface IResillienceLoggerFactory
	{
		IResillienceLogger CreateLogger();
	}
}
