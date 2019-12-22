using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Applicatons.Commands;
using Project.Api.Applicatons.Queries;
using Project.Api.Applicatons.Services;
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
        private IRecommendService _recommendService;
        private IProjectQueries _projectQueries;
        public ProjectController(IMediator mediator, IRecommendService recommendService, IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
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
            if (project==null)
            {
                throw new ArgumentException("错误");
            }
            project.UserId = UserIdentity.UserId;
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
            if (await _recommendService.IsRecommendProject(projectId, UserIdentity.UserId))
            {
                return BadRequest("无权限");
            }
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
        /// <summary>
        /// 我的项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetProjects()
        {
            return Ok(await _projectQueries.GetProjectsByUserId(UserIdentity.UserId));
        }
        /// <summary>
        /// 我的项目详细
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("my/{projectId}")]
        public async Task<IActionResult> GetProjectDetail(int projectId)
        {
            var project= await _projectQueries.GetProjectDetail(projectId);
            if (project.UserId==UserIdentity.UserId)
            {
                return Ok(project);
            }
            else
            {
                return BadRequest("无权限");
            }
        }
        /// <summary>
        /// 推荐项目详细
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recommend/{projectId}")]
        public async Task<IActionResult> GetRecommendProjectDetail(int projectId)
        {
            if (await _recommendService.IsRecommendProject(projectId, UserIdentity.UserId))
            {
                var project = await _projectQueries.GetProjectDetail(projectId);
                return Ok(project);
            }
            else
            {
                return BadRequest("无权限");
            }
        }
    }
}