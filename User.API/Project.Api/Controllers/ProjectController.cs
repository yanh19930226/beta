using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Applicatons.Commands;
using Project.Domain.AggregatesModel;

namespace Project.Api.Controllers
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Route("api/project")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private IMediator _mediator;
        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// 创建项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProjec([FromBody]Domain.AggregatesModel.Project project)
        {
            var command = new CreateProjectCommand()
            {
                Project = project
            };
            var result=await _mediator.Send(command);
            return Ok(result);
        }
        /// <summary>
        /// 查看项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("view/{projectId}")]
        public async Task<IActionResult> ViewProjec(int projectId)
        {
            var command = new ViewProjectCommand()
            {
                ProjectId= projectId,
                UserId=UserIdentity.UserId,
                UserName=UserIdentity.Name,
                Avatar=UserIdentity.Avatar
            };
            await _mediator.Send(command);
            return Ok();
        }
        /// <summary>
        /// 加入项目
        /// </summary>
        /// <param name="contributor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("join")]
        public async Task<IActionResult> JoinProject(ProjectContributor contributor)
        {
            var command = new JoinProjectCommand()
            {
                Contributor= contributor
            };
            await _mediator.Send(command);
            return Ok();
        }
    }
}