using MediatR;
using Microsoft.Extensions.Logging;
using Resillience.EventBus.Abstractions;
using ServiceB.DomainEvents.Tests;
using ServiceB.IntegrationEvents.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceB.DomainEventHandlers.Tests
{
    public class CreateTestDomainEventHandler : INotificationHandler<CreateTestDomainEvent>
    {
        private readonly ILogger<CreateTestDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;
        public CreateTestDomainEventHandler(IEventBus eventBus, ILogger<CreateTestDomainEventHandler> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }
        public Task Handle(CreateTestDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateTestDomainEventHandler");
            //var eventModel = new TestIntegrationEventModel {
            //    Id=1,
            //    Name="我是测试"
            //};

            var eventModel = new TestIntegrationEvent
            {
                Ids = 1,
                Name = "我是测试"
            };

            _eventBus.Publish(eventModel);
            return Task.CompletedTask;
        }
    }
}
