using Resillience.SmsService.AliSms.SDK;
using Resillience.SmsService.AliSms.SDK.Models;
using Resillience.SmsService.AliSms.SDK.Models.SendSms;
using Resillience.Util.ResillienceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Infra.Services
{
    public class AliyunSmsService : ISmsService
    {
        private readonly AliSmsClient _aliSmsClient;
        public  AliyunSmsService(AliSmsClient aliSmsClient)
        {
            _aliSmsClient = aliSmsClient;
        }

        public async Task<BaseResponse<SendSmsResponce>> SendMessage(string PhoneNumbers, string SignName, string TemplateCode, string TemplateParam)
        {
            
            var res = await  _aliSmsClient.SendSms(new SendSmsRequest
            {
                PhoneNumbers= PhoneNumbers,
                SignName= SignName,
                TemplateCode= TemplateCode,
                TemplateParam= TemplateParam
            });
            return res;
        }

        ResillienceResult<dynamic> ISmsService.SendMessage(string PhoneNumbers, string SignName, string TemplateCode, string TemplateParam)
        {
            throw new NotImplementedException();
        }
    }
}
