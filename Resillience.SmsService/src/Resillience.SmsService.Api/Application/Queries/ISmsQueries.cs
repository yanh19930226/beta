using Resillience.SmsService.Abstractions.DTOs.RequestsDTOs;
using Resillience.SmsService.Abstractions.DTOs.ResponceDTOs;
using Resillience.Util.ResillienceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.Queries
{
    public interface ISmsQueries
    {
        ResillienceResult<SmsReseponceDTO> GetById(long id);
        ResillienceResult<IQueryable<SmsReseponceDTO>> SearchMessage(SearchMessageRequestDTO req);
    }
}
