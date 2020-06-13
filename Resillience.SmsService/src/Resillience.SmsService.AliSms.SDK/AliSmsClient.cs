using Flurl.Http;
using Newtonsoft.Json;
using Resillience.SmsService.AliSms.SDK.Models;
using Resillience.SmsService.AliSms.SDK.Models.SendSms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Resillience.SmsService.AliSms.SDK
{
    public class AliSmsClient : AbstractClient
    {
        public AliSmsClient(string accessKeyId, string accessSecret, ProtocolType protocolType, Endpoint endpoint)
            : base(accessKeyId, accessSecret, protocolType, endpoint)
        {
            
        }
        public async Task<BaseResponse<SendSmsResponce>> SendSms(SendSmsRequest req)
        {
                return  await this.RequestAsync(req);
        }
    }
}
