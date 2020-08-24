using Resillience.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.IntegrationEvents.Posts
{
    public class PostIntegrationEvent : IntegrationEvent<PostIntegrationEventModel>
    {
        public PostIntegrationEvent(PostIntegrationEventModel eventData) : base(eventData)
        {

        }
    }
}
