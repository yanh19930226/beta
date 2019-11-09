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
        Task<bool> UpdateContactInfo(BaseUserInfo user, CancellationToken cancellationToken);
        Task<bool> AddContact(int userId,BaseUserInfo user, CancellationToken cancellationToken);
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
