using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.TencentSms.SDK.Common
{
    public class JsonResponseModel<T>
    {
        [JsonProperty("Response")]
        public T Response { get; set; }
    }
}
