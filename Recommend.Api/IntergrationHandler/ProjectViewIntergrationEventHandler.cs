using DotNetCore.CAP;
using Recommend.Api.IntergrationEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.Api.IntergrationHandler
{
    public class ProjectViewIntergrationEventHandler : ICapSubscribe
    {
        public ProjectViewIntergrationEventHandler()
        {
        }
        public Task CreateRecommendProject(ProjectViewIntergrationEvent @event)
        {
            return null;
        }
    }
}
