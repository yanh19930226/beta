using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.AliSms.SDK.Models.SendSms
{
    public class SendSmsRequest: BaseRequest<BaseResponse<SendSmsResponce>>
    {
        public string PhoneNumbers { get; set; }
        public string SignName { get; set; } 
        public string TemplateCode { get; set; } 
        public string TemplateParam { get; set; }
        public override string Action => "SendSms";
    }

    public class SendSmsResponce
    {
        public string BizId { get; set; }
    }
}
