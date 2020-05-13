using Resilience.Zeus.Domain.Core.Models;
using ServiceB.DomainEvents.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ServiceB.Models
{
    public class TestModel:Entity
    {
        public TestModel()
        {
            this.AddDomainEvent(new CreateTestDomainEvent(this));
        }

        public string Name { get; set; }
    }
}
