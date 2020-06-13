using Resillience.SmsService.Abstractions.DTOs.ResponceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.Queries
{
    public interface ISmsQueries
    {
        SmsReseponceDTO GetById(long id);
        IQueryable<SmsReseponceDTO> SearchMessage();
    }
}
