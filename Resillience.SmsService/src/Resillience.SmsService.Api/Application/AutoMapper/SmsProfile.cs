using AutoMapper;
using Resillience.SmsService.Abstractions.DTOs.ResponceDTOs;
using Resillience.SmsService.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Application.AutoMapper
{
    public class SmsProfile:Profile
    {
        public SmsProfile()
        {
            CreateMap<SmsMessage, SmsReseponceDTO>();
        }
    }
}
