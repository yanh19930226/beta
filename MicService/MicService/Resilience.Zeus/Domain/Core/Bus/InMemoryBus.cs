using MediatR;
using Resilience.Zeus.Domain.Core.Commands;
using Resilience.Zeus.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resilience.Zeus.Domain.Core.Bus
{
	public sealed class InMemoryBus : IMediatorHandler
	{
		private readonly IMediator _mediator;

		public InMemoryBus(IMediator mediator)
		{
			_mediator = mediator;
		}

		public Task<T> SendCommandAsync<T>(Command<T> command)
		{
			return _mediator.Send(command);
		}

		public Task RaiseEvent<T>(T @event) where T : Event
		{
			return _mediator.Publish(@event);
		}
	}
}
