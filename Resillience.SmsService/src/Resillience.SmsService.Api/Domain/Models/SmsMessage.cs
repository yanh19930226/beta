using Resilience.Zeus.Domain.Core.Models;
using Resillience.SmsService.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Domain.Models
{
    public class SmsMessage:Entity
    {
        public string Content { get; set; }

        public SmsEnums.SmsType Type { get; set; }

        public SmsEnums.SmsStatus Status { get; set; }
        public List<string> Mobiles { get; set; }
        public int SendCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public DateTime? TimeSendTime { get; set; }
        public bool IsDel { get; set; }
    }
}
