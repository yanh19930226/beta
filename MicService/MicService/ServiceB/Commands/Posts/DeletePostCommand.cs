using Resilience.Zeus.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Commands.Posts
{
    public class DeletePostCommand : Command
    {
        public DeletePostCommand(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
