using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Resillience.EventBus.Abstractions;
using Resillience.EventBus.Events;
using Resillience.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.EventBus.Infrastructure
{
	public class ResillienceEventHandler<TEventHandler, TEvent> : IIntegrationEventHandler<TEvent>, IIntegrationEventHandler where TEvent : IntegrationEvent
	{
		public IStringLocalizer<TEventHandler> G
		{
			get;
			set;
		}

		public IResillienceLogger<TEventHandler> Logger
		{
			get;
			set;
		}

		public virtual async Task Handle(TEvent @event)
		{
			Logger.Information("{@p}", @event);
		}
	}
}
