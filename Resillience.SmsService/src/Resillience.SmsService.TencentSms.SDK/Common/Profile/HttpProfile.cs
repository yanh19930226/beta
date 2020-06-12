using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.TencentSms.SDK.Common.Profile
{
    /// <summary>
    /// HTTP profiles.
    /// </summary>
    public class HttpProfile
    {
        /// <summary>
        /// HTTPS protocol.
        /// </summary>
        public static readonly string REQ_HTTPS = "https://";

        /// <summary>
        /// HTTP protocol.
        /// </summary>
        public static readonly string REQ_HTTP = "http://";

        /// <summary>
        /// HTTP method POST.
        /// </summary>
        public static readonly string REQ_POST = "POST";

        /// <summary>
        /// HTTP method GET.
        /// </summary>
        public static readonly string REQ_GET = "GET";

        /// <summary>
        /// Time unit, 60 seconds.
        /// </summary>
        public static readonly int TM_MINUTE = 60;

        public HttpProfile()
        {
            this.ReqMethod = REQ_POST;
            this.Endpoint = null;
            this.Protocol = REQ_HTTPS;
            this.Timeout = TM_MINUTE;
        }

        /// <summary>
        /// HTTP request method.
        /// </summary>
        public string ReqMethod { get; set; }

        /// <summary>
        /// Service endpoint, or domain name.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// HTTP protocol.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// HTTP request timeout value, in seconds.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// HTTP proxy settings.
        /// </summary>
        public string WebProxy { get; set; }
    }
}
