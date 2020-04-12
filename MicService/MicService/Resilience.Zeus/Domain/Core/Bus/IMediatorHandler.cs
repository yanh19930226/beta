using Resilience.Zeus.Domain.Core.Commands;
using Resilience.Zeus.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resilience.Zeus.Domain.Core.Bus
{
	public interface IMediatorHandler
	{
		Task RaiseEvent<T>(T @event) where T : Event;

		Task<T> SendCommandAsync<T>(Command<T> command);
	}
}
