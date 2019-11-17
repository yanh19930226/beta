using Contact.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.Api.Data
{
    public interface IContactRepository
    {
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UpdateContactInfo(UserIdentity user, CancellationToken cancellationToken);
        /// <summary>
        /// 添加为好友
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddContact(int userId, UserIdentity user, CancellationToken cancellationToken);
        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<List<Models.Contact>> GetContactAsync(int userid, CancellationToken cancellationToken);
        /// <summary>
        /// 更新好友标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<bool> TagContactAsync(int userId,int contactId,List<string> tags, CancellationToken cancellationToken);
    }
}
