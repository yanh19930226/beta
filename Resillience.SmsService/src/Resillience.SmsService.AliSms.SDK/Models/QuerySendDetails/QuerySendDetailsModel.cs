using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.AliSms.SDK.Models.QuerySendDetails
{
    public class QuerySendDetailsRequest:BaseRequest<BaseResponse<QuerySendDetailsResponce>>
    {
        public long CurrentPage { get; set; }
        public long PageSize { get; set; }
        public string PhoneNumbers { get; set; }
        public string SendDate { get; set; }
        public override string Action => throw new NotImplementedException();
    }

    public class QuerySendDetailsResponce
    {
        public List<SmsSendDetailDTO> SmsSendDetailDTOs { get; set; }
        public string TotalCount { get; set; }
    }

    public class SmsSendDetailDTO
    {
        public string Content { get; set;}
        public string ErrCode { get; set;}
        public string OutId { get; set;}
        public string PhoneNum { get; set;}
        public string ReceiveDate { get; set;}
        public string SendDate { get; set; }
        public long SendStatus { get; set; }
        public long TemplateCode { get; set; }
    }
}
