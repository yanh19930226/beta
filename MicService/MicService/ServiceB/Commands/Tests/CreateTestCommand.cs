using Resilience.Zeus.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Commands.Tests
{
    public class CreateTestCommand : Command
    {

        public CreateTestCommand(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
