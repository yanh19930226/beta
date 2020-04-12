using Resillience.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.EventBus.RabbitMQ.EventBusRabbitMQ
{
	public interface IIntegrationEventService
	{
		Task PublishThroughEventBusAsync(IntegrationEvent evt);
	}
}
