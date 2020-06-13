using AutoMapper;
using Resilience.Zeus.Domain.Interfaces;
using Resillience.SmsService.Abstractions.DTOs.ResponceDTOs;
using Resillience.SmsService.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.Queries
{
    public class SmsQueries : ISmsQueries
    {
        private readonly IMapper _mapper;
        public readonly IRepository<SmsMessage> _smsMessageRepository;
        public  SmsQueries(IRepository<SmsMessage> smsMessageRepository, IMapper mapper)
        {
            _smsMessageRepository = smsMessageRepository;
            _mapper = mapper;
        }
        public SmsReseponceDTO GetById(long id)
        {
           return  _mapper.Map<SmsReseponceDTO>(_smsMessageRepository.GetByIdAsync(id));
        }

        public IQueryable<SmsReseponceDTO> SearchMessage()
        {
            
        }
    }
}
