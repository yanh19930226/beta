using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Dto;

namespace Contact.Api.Service
{
    public class UserService : IUserService
    {
        public Task<UserIdentity> GetBaseUserInfoAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
