using Resilience.Zeus.Domain.Core.Events;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.DomainEvents.Tests
{
    public class CreateTestDomainEvent: Event
    {
        public CreateTestDomainEvent(TestModel testModel)
        {
            TestModel = testModel;
        }

        public TestModel TestModel { get; set; }
    }
}
