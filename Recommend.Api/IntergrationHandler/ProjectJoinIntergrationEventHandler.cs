using DotNetCore.CAP;
using Recommend.Api.IntergrationEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.Api.IntergrationHandler
{
    public class ProjectJoinIntergrationEventHandler: ICapSubscribe
    {
        public ProjectJoinIntergrationEventHandler()
        {
        }
        public Task CreateRecommendProject(ProjectJoinIntergrationEvent @event)
        {
            return null;
        }
    }
}
