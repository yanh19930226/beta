using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resillience.EventBus.Abstractions;

namespace Resillience.SmsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        //private readonly SmsService _smsService;
        private readonly IEventBus _eventBus;
        public SmsController(/*SmsService smsService,*/ IEventBus eventBus)
        {
            //_smsService = smsService;
            _eventBus = eventBus;
        }
        /// <summary>
        /// 获取短信记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            //if (string.IsNullOrEmpty(id))
            //    return NotFound();

            //var smsService = _smsService.Get(id);
            //return smsService.Sms;
            return null;
        }
    }
}