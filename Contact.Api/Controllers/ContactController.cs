using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Data;
using Contact.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : BaseController
    {
        private IContactApplyRequestRepository _contactApplyRequestRepository;
        private IUserService _userService;
        public  ContactController(IContactApplyRequestRepository contactApplyRequestRepository, IUserService userService)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
        }
        /// <summary>
        /// 获取当前用户好友申请列表
        /// </summary>
        /// <returns></returns>
        [Route("apply-request")]
        [HttpGet]
        public async Task<IActionResult> GetApplyRequestList()
        {
            var result=await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId);
            return Ok(result);
        }
        /// <summary>
        /// 请求添加别人为好友(申请人自己)
        /// </summary>
        /// <param name="userId">被申请用户Id</param>
        /// <returns></returns>
        [Route("apply-request")]
        [HttpPost]
        public async Task<IActionResult> AddApplyRequest(int userId)
        {
            var baseUserInfo = await _userService.GetBaseUserInfoAsync(userId);
            if (baseUserInfo==null)
            {
                throw new Exception("用户参数错误");
            }
            var result=await _contactApplyRequestRepository.AddRequestAsync(new Models.ContactApplyRequest
            {
                AppliedId=UserIdentity.UserId,
                UserId=userId,
                Name= baseUserInfo.Name,
                Company=baseUserInfo.Company,
                Title=baseUserInfo.Title,
                Avatar=baseUserInfo.Avatar,
                CreateTime=DateTime.Now
            });
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        /// <summary>
        /// 同意或者拒绝好友申请
        /// </summary>
        /// <returns></returns>
        [Route("apply-request")]
        [HttpPut]
        public async Task<IActionResult> ApprovalApplyRequest(int applierId)
        {
            var result = await _contactApplyRequestRepository.ApprovalAsync(applierId);
            if (result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
