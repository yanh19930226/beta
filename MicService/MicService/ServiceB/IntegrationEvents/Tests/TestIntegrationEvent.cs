using Resillience.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.IntegrationEvents.Tests
{
    public class TestIntegrationEvent : IntegrationEvent<TestIntegrationEventModel>
    {
        public TestIntegrationEvent(TestIntegrationEventModel eventData) : base(eventData)
        {

        }
    }
}
