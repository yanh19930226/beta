using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Models;

namespace Contact.Api.Data
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRepository
    {
        private readonly ContactContext _contactContext;
        public  MongoContactApplyRequestRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        /// <summary>
        /// 请求添加好友
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<bool> AddRequestAsync(ContactApplyRequest request)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 是否同意好友申请
        /// </summary>
        /// <param name="appliedId"></param>
        /// <returns></returns>
        public Task<bool> ApprovalAsync(int appliedId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取当前用户的好友申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<bool> GetRequestListAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
