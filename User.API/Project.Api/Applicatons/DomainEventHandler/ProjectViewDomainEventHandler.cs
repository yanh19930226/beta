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
    public class ProjectViewDomainEventHandler : INotificationHandler<ProjectViewedEvent>
    {
        private readonly ICapPublisher _capBus;
        public ProjectViewDomainEventHandler(ICapPublisher capBus)
        {
            _capBus = capBus;
        }
        public Task Handle(ProjectViewedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectViewIntergrationEvent() {
               Viewer= notification.Viewer
            };
            _capBus.Publish("ProjectView", @event);
            return Task.CompletedTask;
        }
    }
}
