﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Resilience.Zeus.Domain.Core.Bus;
using Resilience.Zeus.Domain.Interfaces;
using ServiceB.Commands.Tests;
using ServiceB.DTO.Test;
using ServiceB.Models;
using ServiceB.Queries;

namespace ServiceB.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("api/test")]
    [ApiController]
    //[Authorize]
    public class TestsController : ControllerBase
    {
        private readonly ITestQueries _q;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly ILogger<TestsController> _logger;
        public readonly IRepository<TestModel> _testRepository;
        public TestsController(ILogger<TestsController> logger, ITestQueries q, IRepository<TestModel> testRepository,IMapper mapper, IMediatorHandler bus)
        {
            _q = q;
            _logger = logger;
            _testRepository = testRepository;
            _mapper = mapper;
            _bus = bus;
        }
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var res = "测试";
            //var res = _q.GetAll().ProjectTo<TestDTO>(_mapper.ConfigurationProvider)
            //      .ToList(); ;
            _logger.LogInformation(DateTime.Now.ToString()+"测试时间", null);
            return Ok(res);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public async Task<bool> Create([FromBody]CreateDTO model)
        {
            var command = new CreateTestCommand(model.Name);
            return await _bus.SendCommandAsync(command);
        }
    }
}
