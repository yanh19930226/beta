using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applicatons.Commands
{
    public class CreateProjectCommand: IRequest<Project.Domain.AggregatesModel.Project>
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}
