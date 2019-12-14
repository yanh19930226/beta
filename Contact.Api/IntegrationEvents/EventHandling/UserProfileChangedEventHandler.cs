using Contact.Api.Data;
using Contact.Api.IntegrationEvents.Events;
using DotNetCore.CAP;
using Resilience.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.Api.IntegrationEvents.EventHandling
{
    public class UserProfileChangedEventHandler:ICapSubscribe
    {
        public IContactRepository _contactRepository;
        public UserProfileChangedEventHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        [CapSubscribe("beta_userprofilechange")]
        public async Task UpdateContactInfo(UserProfileChangedEvent @event)
        {
            var token = new CancellationToken();
            await _contactRepository.UpdateContactInfo(new UserIdentity()
            {
                Avatar=@event.Avatar,
                Name= @event.Name,
                Title= @event.Title,
                Company= @event.Company,
                UserId= @event.UserId
            }, token);
        }
    }
}
