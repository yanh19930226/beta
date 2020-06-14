using Resillience.EventBus.Abstractions;
using Resillience.SmsService.Api.Application.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.IntegrationEventHandlers
{
    public class SendMessageIntegrationEventHandler : IIntegrationEventHandler<SendMessageIntegrationEvent>
    {
        public Task Handle(SendMessageIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
