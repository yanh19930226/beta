using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.Abstractions.Enums
{
    public class SmsEnums
    {
        public enum MarketingSmsStatus
        {
            待发送,
            已发送,
            发送失败
        }

        public enum MarketingSmsAccountType
        {
            手机号,
            用户Id
        }

        public enum SmsType
        {
            Aliyun,
            Tencent
        }

        public enum SmsStatus
        {
            失败 = -1,
            待处理 = 0,
            成功 = 1
        }
    }
}
