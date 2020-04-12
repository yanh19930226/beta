using Resilience.Zeus.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resilience.Zeus.Domain.Core.Commands
{
	public abstract class Command<T> : Message<T>
	{
		public DateTime Timestamp
		{
			get;
			private set;
		}

		protected Command()
		{
			Timestamp = DateTime.Now;
		}
	}
	public abstract class Command : Command<bool>
	{
	}
}
