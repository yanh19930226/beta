using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.TencentSms.SDK.Common.Exceptions
{
    public class TencentCloudSDKException :Exception
    {
        public TencentCloudSDKException(string message)
            : this(message, "")
        {

        }

        public TencentCloudSDKException(string message, string requestId) :
            base(message)
        {
            this.RequestId = requestId;
        }

        /// <summary>
        /// UUID of a request.
        /// </summary>
        public string RequestId { get; private set; }

        public override string ToString()
        {
            return $"message：{this.Message} requestId{RequestId}";
        }
    }
}
