using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.TencentSms.SDK.Common
{
    /// <summary>
    /// Credentials.
    /// </summary>
    public class Credential
    {
        /// <summary>
        /// SecretId, can only be obtained from Tencent Cloud Management Console.
        /// </summary>
        public string SecretId { get; set; }

        /// <summary>
        /// SecretKey, can only be obtained from Tencent Cloud Management Console.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}
