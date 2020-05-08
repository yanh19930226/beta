using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Resillience.Test;

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
        [HttpGet]
        public IActionResult Get()
        {
            var res = _testService.Show();
            _logger.LogInformation("test", null);
            return Ok(res);
        }
    }
}
