using Resillience.SmsService.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.Abstractions.DTOs.RequestsDTOs
{
    /// <summary>
    /// 搜索短信DTO
    /// </summary>
    public class SearchMessageRequestDTO
    {
        /// <summary>
        /// 短信内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 短信运营商
        /// </summary>
        public SmsEnums.SmsType? Type { get; set; }
        /// <summary>
        /// 短信发送状态
        /// </summary>
        public SmsEnums.SmsStatus? Status { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 创建时间Begin
        /// </summary>
        public DateTime? BeganCreateTime { get; set; }
        /// <summary>
        /// 创建时间End
        /// </summary>
        public DateTime? EndCreateTime { get; set; }
        /// <summary>
        /// 发送时间Begin
        /// </summary>
        public DateTime? BeganTimeSendTime { get; set; }
        /// <summary>
        /// 发送时间End
        /// </summary>
        public DateTime? EndTimeSendTime { get; set; }
    }
}
