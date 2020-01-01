using DotNetCore.CAP;
using Recommend.Api.Data;
using Recommend.Api.IntergrationEvent;
using Recommend.Api.Models;
using Recommend.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.Api.IntergrationHandler
{
    public class ProjectCreateIntergrationEventHandler:ICapSubscribe
    {
        private RecommendContext _context;
        private IUserService _userService;
        private IContactService _contactService;
        public ProjectCreateIntergrationEventHandler(RecommendContext context, IUserService userService, IContactService contactService)
        {
            _context = context;
            _userService = userService;
            _contactService = contactService;
        }
        [CapSubscribe("ProjectCreated")]
        public async Task CreateRecommendProject(ProjectCreateIntergrationEvent @event)
        {
            //Rpc获取创建项目的基本信息
            //var baseUserInfo = await _userService.GetBaseUserInfoAsync(@event.UserId);
            //Rpc获取项目创建者的好友
            //var contacts= _contactService.GetContact(@event.UserId);
            //循环遍历添加项目推荐
            var recommend = new ProjectRecommend()
            {
                FromUserId = @event.UserId,
                FromUserName= "yanh",
                FromUserAvatar= "yanh",
                ProjectAvatar = "test",
                ProjectId = @event.ProjectId
            };
            _context.ProjectRecommends.Add(recommend);
            _context.SaveChanges();
        }
    }
}
