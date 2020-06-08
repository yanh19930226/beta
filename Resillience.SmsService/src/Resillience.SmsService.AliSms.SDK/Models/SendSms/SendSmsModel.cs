using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.AliSms.SDK.Models.SendSms
{
    public class SendSmsRequest: BaseRequest<BaseResponse<SendSmsResponce>>
    {
        public string PhoneNumbers { get; set; }
        public string SignName { get; set; } = "严辉";
        public string TemplateCode { get; set; } = "SMS_192532379";
        public override string Action => "SendSms";
    }

    public class SendSmsResponce
    {
        public string BizId { get; set; }
    }
}
