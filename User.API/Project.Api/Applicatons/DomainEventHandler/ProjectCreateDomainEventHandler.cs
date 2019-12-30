﻿using DotNetCore.CAP;
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
    public class ProjectCreateDomainEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private readonly ICapPublisher _capBus;
        public ProjectCreateDomainEventHandler(ICapPublisher capBus)
        {
            _capBus = capBus;
        }
        public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreateIntergrationEvent() {
                ProjectId = notification.Project.Id,
                CreateTime=DateTime.Now,
                UserId= notification.Project.UserId
            };
            _capBus.Publish("ProjectCreated", @event);
            return Task.CompletedTask;
        }
    }
}
