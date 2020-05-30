using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Resilience.Zeus.Domain.Core.Bus;
using Resillience.ResillienceApiResult;
using ServiceB.Commands.Posts;
using ServiceB.DTO.Post;
using ServiceB.Models;
using ServiceB.Queries.PostQueries;

namespace ServiceB.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostQueries _q;
        private readonly IMediatorHandler _bus;
        private readonly IMapper _mapper;
        private readonly ILogger<PostController> _logger;
        public PostController(ILogger<PostController> logger, IPostQueries q,IMapper mapper, IMediatorHandler bus)
        {
            _q = q;
            _logger = logger;
            _mapper = mapper;
            _bus = bus;
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<IQueryable<Post>> Get()
        {
            return _q.GetAll();
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [Route("page")]
        public ApiResult<IQueryable<Post>> GetPage([FromBody]PostPageRequestDTO req)
        {
            return _q.GetPage(req);
        }
        /// <summary>
        /// 列表关联分页
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagejoin")]
        public IActionResult GetPageJoin([FromBody]PostPageRequestDTO req)
        {
            var res = _q.GetPageJoin(req);
            return Ok(res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req">Post创建DTO</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<bool> Create([FromBody]CreatePostDTO req)
        {
            CreatePostCommand command = new CreatePostCommand(req.Title, req.Content, req.BlogId);
            return await _bus.SendCommandAsync(command);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<bool> Update()
        {
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<bool> Delete()
        {
            return true;
        }
    }
}