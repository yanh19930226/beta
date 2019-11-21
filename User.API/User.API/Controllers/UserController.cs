using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Data;
using Microsoft.EntityFrameworkCore;
using User.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using DotNetCore.CAP;
using User.API.Dto;

namespace User.API.Controllers
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ICapPublisher _capBus;
        private UserContext _userContext;
        //private ILogger _logger;
        /// <summary>
        /// 用户服务
        /// </summary>
        /// <param name="userContext"></param>
        public UserController(UserContext userContext, ICapPublisher capBus)
        {
            _capBus = capBus;
            _userContext = userContext;

            //_logger = logger;
        }
        /// <summary>
        /// 用户信息修改事件
        /// </summary>
        /// <param name="user"></param>
        private void UserProfileChangeEvent(AppUser user)
        {
            //如果用户的信息修改
            if (_userContext.Entry(user).Property(nameof(user.Company)).IsModified||
                _userContext.Entry(user).Property(nameof(user.Name)).IsModified||
                _userContext.Entry(user).Property(nameof(user.Title)).IsModified||
                _userContext.Entry(user).Property(nameof(user.Avatar)).IsModified)
            {
                //发送事件
                _capBus.Publish("beta_userprofilechange",new UserIdentity {
                    UserId= user.Id,
                    Company=user.Company,
                    Title=user.Title,
                    Avatar=user.Avatar,
                    Name=user.Name
                });
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [Route("get-user")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userContext.Users
                 .AsNoTracking()
                 .Include(u => u.Properties)
                 .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            if (user == null)
                throw new UserOprationException($"错误的用户上下文id{UserIdentity.UserId}");
            return Json(user);

        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("getuserinfo/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var user = await _userContext.Users
                 .AsNoTracking()
                 .Include(u => u.Properties)
                 .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new UserOprationException($"错误的用户上下文id{UserIdentity.UserId}");
            return Json(new
            {
                UserId = user.Id,
                user.Name,
                user.Company,
                user.Avatar,
                user.Title
            });

        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        [Route("patch-user")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<AppUser> patch)
        {
            var user = await _userContext.Users
               .Include(u => u.Properties)
               .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);
            foreach (var item in user?.Properties)
            {
                _userContext.UserProperties.Remove(item);
            }
            patch.ApplyTo(user);
            foreach (var item in user.Properties)
            {
                _userContext.UserProperties.Add(item);
            }
            using (var transaction = _userContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                UserProfileChangeEvent(user);
                _userContext.Update(user);
                _userContext.SaveChanges();
            }
            return Json(user);
        }
        /// <summary>
        /// 检查或则创建用户
        /// </summary>
        /// <returns></returns>
        [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreate([FromForm]string phone)
        {
            //ToDo:检查手机号码的格式
            var user = _userContext.Users.SingleOrDefault(q => q.Phone == phone);
            if (user == null)
            {
                user = new AppUser { Phone = phone };
                _userContext.Users.Add(user);
                await _userContext.SaveChangesAsync();
            }
            return Ok(new {
                UserId=user.Id,
                user.Name,
                user.Company,
                user.Avatar,
                user.Title
            });
        }
        /// <summary>
        /// 获取用户的标签
        /// </summary>
        /// <returns></returns>
        [Route("tags")]
        [HttpGet]
        public async Task<IActionResult>GetUserTags()
        {
            return Ok(await _userContext.UserTags.Where(u=>u.UserId==UserIdentity.UserId).ToListAsync());
        }
        /// <summary>
        /// 根据手机号码查询用户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public async Task<IActionResult> Search(string phone)
        {
            return Ok(await _userContext.Users.Include(u => u.Properties).SingleOrDefaultAsync(q => q.Phone== phone));
        }
        /// <summary>
        /// 更新用户标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [Route("tags")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserTags([FromBody]List<string> tags)
        {
            var originTags = _userContext.UserTags.Where(q => q.UserId == UserIdentity.UserId);
            var newTags = tags.Except(originTags.Select(t=>t.Tag));
            await _userContext.UserTags.AddRangeAsync(newTags.Select(t => new UserTag
            {
                Tag = t,
                UserId = UserIdentity.UserId
            }));
            await _userContext.SaveChangesAsync();
            return Ok();
        }
    }
}
