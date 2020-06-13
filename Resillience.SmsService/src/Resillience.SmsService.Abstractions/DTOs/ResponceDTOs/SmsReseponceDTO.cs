using Resillience.SmsService.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.Abstractions.DTOs.ResponceDTOs
{
    /// <summary>
    /// 短信返回DTO
    /// </summary>
    public class SmsReseponceDTO
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
        /// 短信发送状态
        /// </summary>
        public SmsEnums.SmsStatus Status { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public List<string> Mobiles { get; set; }
        /// <summary>
        /// 发送次数
        /// </summary>
        public int SendCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? TimeSendTime { get; set; }
    }
}
