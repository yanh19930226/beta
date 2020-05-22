using Microsoft.Extensions.Logging;
using Resillience.EventBus.Abstractions;
using ServiceB.IntegrationEvents.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.IntegrationEventHandlers.Deals
{
    public class DealIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        private readonly ILogger<DealIntegrationEventHandler> _logger;

        public DealIntegrationEventHandler(ILogger<DealIntegrationEventHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(TestIntegrationEvent @event)
        {
            _logger.LogInformation(@event.Ids.ToString());
            _logger.LogInformation(@event.Name.ToString());
            return Task.CompletedTask;
        }
    }
}
