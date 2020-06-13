using AutoMapper;
using AutoMapper.QueryableExtensions;
using Resilience.Zeus.Domain.Interfaces;
using Resillience.SmsService.Abstractions.DTOs.RequestsDTOs;
using Resillience.SmsService.Abstractions.DTOs.ResponceDTOs;
using Resillience.SmsService.Api.Domain.Models;
using Resillience.Util;
using Resillience.Util.ResillienceResult;
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
        public ResillienceResult<SmsReseponceDTO> GetById(long id)
        {
            var result = new ResillienceResult<SmsReseponceDTO>();
            var res= _mapper.Map<SmsReseponceDTO>(_smsMessageRepository.GetByIdAsync(id));
            if (res == null)
            {
                result.IsFailed("数据不存在");
                return result;
            }
            result.IsSuccess(res);
            return result;
        }

        public ResillienceResult<IQueryable<SmsReseponceDTO>> SearchMessage(SearchMessageRequestDTO req)
        {
            var result = new ResillienceResult<IQueryable<SmsReseponceDTO>>();
            var expression = LinqExtensions.True<SmsMessage>();

            if (req.Status.HasValue)
                expression = expression.And(a => a.Status == req.Status.Value);

            if (req.Type.HasValue)
                expression = expression.And(a => a.Type == req.Type.Value);

            if (req.BeganCreateTime.HasValue)
                expression = expression.And(a => a.CreateTime >= req.BeganCreateTime.Value);

            if (req.EndCreateTime.HasValue)
                expression = expression.And(a => a.CreateTime <= req.EndCreateTime.Value);

            if (req.BeganTimeSendTime.HasValue)
                expression = expression.And(a => a.TimeSendTime >= req.BeganTimeSendTime.Value);

            if (req.EndTimeSendTime.HasValue)
                expression = expression.And(a => a.TimeSendTime <= req.EndTimeSendTime.Value);

            if (!string.IsNullOrEmpty(req.Mobile))
                expression = expression.And(a => a.Mobiles.Contains(req.Mobile));

            if (!string.IsNullOrEmpty(req.Content))
                expression = expression.And(a => a.Content.Contains(req.Content));

            var res = _smsMessageRepository.GetAll().Where(expression).ProjectTo<SmsReseponceDTO>(_mapper.ConfigurationProvider);
            if (res == null)
            {
                result.IsFailed("数据不存在");
                return result;
            }
            result.IsSuccess(res);
            return result;
        }
    }
}
