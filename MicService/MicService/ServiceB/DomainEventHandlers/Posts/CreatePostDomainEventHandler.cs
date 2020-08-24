using MediatR;
using Microsoft.Extensions.Logging;
using Resilience.Event.IntegrationEventLog.Services;
using Resilience.Zeus.Domain.Interfaces;
using Resilience.Zeus.Infra.Data.Context;
using Resillience.EventBus.Abstractions;
using Resillience.EventBus.Events;
using ServiceB.DomainEventHandlers.Tests;
using ServiceB.DomainEvents.Posts;
using ServiceB.IntegrationEvents.Posts;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceB.DomainEventHandlers.Posts
{
    public class CreatePostDomainEventHandler : INotificationHandler<CreatePostDomainEvent>
    {
        private readonly ILogger<CreateTestDomainEventHandler> _logger;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly IEventBus _eventBus;
        public CreatePostDomainEventHandler(IEventBus eventBus, ILogger<CreateTestDomainEventHandler> logger, IIntegrationEventLogService eventLogService)
        {
            _eventBus = eventBus;
            _logger = logger;
            _eventLogService = eventLogService;
        }
        public async Task Handle(CreatePostDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateTestDomainEventHandler");

            var eventModel = new PostIntegrationEventModel
            {
                Name = notification.Post.Title
            };
            var @event=new PostIntegrationEvent(eventModel);
            await SaveEventAsync(@event);
            await PublishThroughEventBusAsync(@event);
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("Publishing integration event: {IntegrationEventId_published}- ({@IntegrationEvent})", evt.Id, evt);
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
            await _eventLogService.SaveEventAsync(evt);
        }
    }
}
