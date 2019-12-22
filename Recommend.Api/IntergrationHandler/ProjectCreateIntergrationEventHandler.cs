using DotNetCore.CAP;
using Recommend.Api.IntergrationEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.Api.IntergrationHandler
{
    public class ProjectCreateIntergrationEventHandler:ICapSubscribe
    {
        public ProjectCreateIntergrationEventHandler()
        {
        }
        public Task CreateRecommendProject(ProjectCreateIntergrationEvent @event)
        {
            return null;
        }
    }
}
