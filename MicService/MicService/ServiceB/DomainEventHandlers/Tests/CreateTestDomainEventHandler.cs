﻿using MediatR;
using Microsoft.Extensions.Logging;
using Resilience.Event.IntegrationEventLog.Services;
using Resilience.Event.IntegrationEventLog.Utilities;
using Resilience.Zeus.Infra.Data.Context;
using Resillience.EventBus.Abstractions;
using Resillience.EventBus.Events;
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
        private readonly IIntegrationEventLogService _eventLogService;
        public ZeusContext _zeusContext;
        private readonly IEventBus _eventBus;
        public CreateTestDomainEventHandler(IEventBus eventBus, ILogger<CreateTestDomainEventHandler> logger, IIntegrationEventLogService eventLogService, ZeusContext zeusContext)
        {
            _eventBus = eventBus;
            _logger = logger;
            _eventLogService = eventLogService;
            _zeusContext = zeusContext;
        }
        public async Task Handle(CreateTestDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateTestDomainEventHandler");


            var eventModel = new TestIntegrationEventModel
            {
                Id = 1,
                Name = "我是测试"
            };

            //_eventBus.Publish(new TestIntegrationEvent(eventModel));
            await SaveEventAsync(new TestIntegrationEvent(eventModel));
            await PublishThroughEventBusAsync(new TestIntegrationEvent(eventModel));
            //return await CommitAsync();
            //return Task.CompletedTask;
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("Publishing integration event: {IntegrationEventId_published}- ({@IntegrationEvent})", evt.Id,evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId}  - ({@IntegrationEvent})", evt.Id, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("Saving integrationEvent: {IntegrationEventId}", evt.Id);

            await _eventLogService.SaveEventAsync(evt/*, _zeusContext.Database.CurrentTransaction*/);
        }
    }
}
