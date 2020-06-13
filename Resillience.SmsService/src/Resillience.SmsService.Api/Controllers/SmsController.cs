using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resillience.EventBus.Abstractions;
using Resillience.SmsService.Abstractions.DTOs.RequestsDTOs;
using Resillience.SmsService.Api.Application.Queries;

namespace Resillience.SmsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        //private readonly SmsService _smsService;
        private readonly ISmsQueries _smsQueries;
        private readonly IEventBus _eventBus;
        public SmsController(ISmsQueries smsQueries, IEventBus eventBus)
        {
            _smsQueries = smsQueries;
            _eventBus = eventBus;
        }
        /// <summary>
        /// 获取短信记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Ok(_smsQueries.GetById(id));
        }
        public IActionResult SendMessage([FromBody]SendMessageRequestDTO req)
        {
            return Ok();
        }
        [HttpPost]
        [Route("search")]
        public IActionResult SearchMessage([FromBody] SearchMessageRequestDTO req)
        {
            return Ok();
        }
    }
}