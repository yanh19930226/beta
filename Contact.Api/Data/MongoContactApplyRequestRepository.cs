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
        public Task<bool> AddRequestAsync(ContactApplyRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApprovalAsync(int appliedId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetRequestListAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
