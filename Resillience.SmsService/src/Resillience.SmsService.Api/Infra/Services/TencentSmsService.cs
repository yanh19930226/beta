using Resillience.SmsService.TencentSms.SDK;
using Resillience.SmsService.TencentSms.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Infra.Services
{
    public class TencentSmsService : ISmsService
    {
        private readonly TencentSmsClient _tencentSmsClient;
        public TencentSmsService(TencentSmsClient tencentSmsClient)
        {
            _tencentSmsClient = tencentSmsClient;
        }
        public async Task<SendSmsResponse> SendMessage(string PhoneNumbers, string SignName, string TemplateCode, string TemplateParam)
        {

            var res = await _tencentSmsClient.SendSms(new SendSmsRequest
            {
                PhoneNumberSet = PhoneNumbers,
                Sign = SignName,
                TemplateID = TemplateCode,
                TemplateParamSet = TemplateParam
            });
            return res;
        }
    }
}
