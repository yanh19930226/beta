using Resillience.EventBus.Events;
using Resillience.SmsService.Abstractions.IntegrationEventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.IntegrationEvents
{
    public class SendMessageIntegrationEvent : IntegrationEvent<SendMessageIntegrationEventModel>
    {
        public SendMessageIntegrationEvent(SendMessageIntegrationEventModel eventData) : base(eventData)
        {

        }
    }
}
