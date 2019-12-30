using DotNetCore.CAP;
using MediatR;
using Project.Api.Applicatons.IntergrationEvent;
using Project.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Api.Applicatons.DomainEventHandler
{
    public class ProjectJoinDomainEventHandler : INotificationHandler<ProjectJoinedEvent>
    {
        private readonly ICapPublisher _capBus;
        public ProjectJoinDomainEventHandler(ICapPublisher capBus)
        {
            _capBus = capBus;
        }
        public Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinIntergrationEvent() {
                Contributor = notification.Contributor
            };
            _capBus.Publish("ProjectJoin", @event);
            return Task.CompletedTask;
        }
    }
}
