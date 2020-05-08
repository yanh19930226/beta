using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Resillience.Test;
using ServiceB.Models;

namespace ServiceB.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ILogger<TestsController> _logger;
        public TestsController(ITestService testService, ILogger<TestsController> logger)
        {
            _testService = testService;
            _logger = logger;
        }
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var res = _testService.Show();
            _logger.LogInformation("test", null);
            return Ok(res);
        }
        /// <summary>
        /// 测试参数
        /// </summary>
        /// <param name="model">post参数</param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public IActionResult AddTest([FromBody]TestModel model)
        {
            return Ok();
        }
    }
}
