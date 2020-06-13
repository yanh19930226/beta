using Resillience.SmsService.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.Abstractions.DTOs.RequestsDTOs
{
    /// <summary>
    /// 发送短信请求类
    /// </summary>
    public class SendMessageRequestDTO
    {
        /// <summary>
        /// 短信内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 短信运营商
        /// </summary>
        public SmsEnums.SmsType Type { get; set; }
        /// <summary>
        /// 发送手机号码
        /// </summary>
        public List<string> Mobiles { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? TimeSendDateTime { get; set; }
    }
}
