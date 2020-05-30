using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceB.Queries.PostQueries;

namespace ServiceB.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostQueries _q;
        private readonly IMapper _mapper;
        private readonly ILogger<PostController> _logger;
        public PostController(ILogger<PostController> logger, IPostQueries q,IMapper mapper)
        {
            _q = q;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var res = _q.GetAll().ToList(); ;
            _logger.LogInformation("list", null);
            return Ok(res);
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public IActionResult GetPage()
        {
            var res = _q.GetPage();
            _logger.LogInformation("page", null);
            return Ok(res);
        }
        /// <summary>
        /// 列表关联分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pagejoin")]
        public IActionResult GetPageJoin()
        {
            var res = _q.GetPageJoin();
            _logger.LogInformation("page", null);
            return Ok(res);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create()
        {
            return Ok();
        }
        [HttpPut]
        [Route("update")]
        public IActionResult Update()
        {
            return Ok();
        }
        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete()
        {
            return Ok();
        }
    }
}