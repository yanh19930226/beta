using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resillience.EventBus.Abstractions;
using Resillience.SmsService.Abstractions.DTOs.RequestsDTOs;
using Resillience.SmsService.Abstractions.DTOs.ResponceDTOs;
using Resillience.SmsService.Api.Application.Commands;
using Resillience.SmsService.Api.Application.Queries;
using Resillience.Util.ResillienceResult;

namespace Resillience.SmsService.Api.Controllers
{
    /// <summary>
    /// 短信服务
    /// </summary>
    [Route("api/sms")]
    [ApiController]
    public class SmsController : ControllerBase
    {
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
        public ResillienceResult<SmsReseponceDTO> Get(long id)
        {
            return _smsQueries.GetById(id);
        }
        /// <summary>
        ///条件搜索短信记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public ResillienceResult<IQueryable<SmsReseponceDTO>> SearchMessage([FromBody] SearchMessageRequestDTO req)
        {
            var res = _smsQueries.SearchMessage(req);
            return res;
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send")]
        public IActionResult SendMessage([FromBody] SendMessageRequestDTO req)
        {
            SendMessageCommand command = new SendMessageCommand();
            return Ok();
        }
    }
}