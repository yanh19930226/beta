using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.Api.Services
{
    public interface IContactService
    {
        string GetContact(int userId);
    }
}
