using Contact.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.Api.Data
{
    public interface IContactApplyRequestRepository
    {
        
        /// <summary>
        /// 申请好友请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddRequestAsync(ContactApplyRequest request, CancellationToken cancellationToken);
        /// <summary>
        ///通过好友申请
        /// </summary>
        /// <param name="appliedId">申请用户id</param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int userId,int appliedId, CancellationToken cancellationToken);
        /// <summary>
        /// 获取用户所有申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken);
        
    }
}
