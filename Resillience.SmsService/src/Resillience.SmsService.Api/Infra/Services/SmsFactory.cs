using Resillience.Exceptions;
using Resillience.SmsService.Abstractions.Enums;
using Resillience.SmsService.AliSms.SDK;
using Resillience.SmsService.TencentSms.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Infra.Services
{
    public class SmsFactory
    {
        private readonly AliyunSmsService _aliyunSmsService;
        private readonly TencentSmsService _tencentSmsService;
        public SmsFactory(AliyunSmsService aliyunSmsService, TencentSmsService tencentSmsService)
        {
            _aliyunSmsService = aliyunSmsService;
            _tencentSmsService = tencentSmsService;
        }
        public ISmsService Create(SmsEnums.SmsType type)
        {
            switch (type)
            {
                case SmsEnums.SmsType.Aliyun: return _aliyunSmsService;
                case SmsEnums.SmsType.Tencent: return _tencentSmsService;
                default: throw new ResillienceException("无法识别的type");
            }
        }
    }
}
