using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.TencentSms.SDK.Common.Profile
{
    /// <summary>
    /// Client profiles.
    /// </summary>
    public class ClientProfile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="signMethod">Signature process method.</param>
        /// <param name="httpProfile">HttpProfile instance.</param>
        public ClientProfile(string signMethod, HttpProfile httpProfile)
        {
            this.SignMethod = signMethod;
            this.HttpProfile = httpProfile;
        }

        public ClientProfile(string signMethod)
            : this(signMethod, new HttpProfile())
        {

        }

        public ClientProfile()
        {
            SignMethod = SIGN_SHA256;
            HttpProfile = new HttpProfile();
        }

        /// <summary>
        /// HTTP profiles, refer to <seealso cref="HttpProfile"/>
        /// </summary>
        public HttpProfile HttpProfile { get; set; }

        /// <summary>
        /// Signature process method.
        /// </summary>
        public string SignMethod { get; set; }


        /// <summary>
        /// Signature process version 1, with HmacSHA1.
        /// </summary>
        public const string SIGN_SHA1 = "HmacSHA1";

        /// <summary>
        /// Signature process version 1, with HmacSHA256.
        /// </summary>
        public static string SIGN_SHA256 = "HmacSHA256";

        /// <summary>
        /// Signature process version 3, with TC3-HMAC-SHA256.
        /// </summary>
        public static string SIGN_TC3SHA256 = "TC3-HMAC-SHA256";
    }
}
