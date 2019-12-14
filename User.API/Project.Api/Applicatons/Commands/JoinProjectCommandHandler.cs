using MediatR;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Api.Applicatons.Commands
{
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand>
    {
        private IProjectRepository _projectRepository;
        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);
            if (project == null)
            {
                throw new ProjectDomainException("异常");
            }
            project.AddContributor(request.Contributor);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
