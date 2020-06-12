using Resillience.SmsService.TencentSms.SDK.Common.Profile;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Resillience.SmsService.TencentSms.SDK.Common
{
    /// <summary>
    /// Signature helper class.
    /// </summary>
    public class SignHelper
    {
        ///<summary>Generate signature.</summary>
        ///<param name="signKey">Credential SecretKey.</param>
        ///<param name="secret">Plain text to be signed.</param>
        ///<returns>Signature.</returns>
        public static string Sign(string signKey, string secret, string SignatureMethod)
        {
            string signRet = string.Empty;
            if (SignatureMethod == ClientProfile.SIGN_SHA256)
            {
                using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(signKey)))
                {
                    byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                    signRet = Convert.ToBase64String(hash);
                }
            }
            else if (SignatureMethod == ClientProfile.SIGN_SHA1)
            {
                using (HMACSHA1 mac = new HMACSHA1(Encoding.UTF8.GetBytes(signKey)))
                {
                    byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                    signRet = Convert.ToBase64String(hash);
                }
            }
            return signRet;
        }

        public static string SHA256Hex(string s)
        {
            using (SHA256 algo = SHA256.Create())
            {
                byte[] hashbytes = algo.ComputeHash(Encoding.UTF8.GetBytes(s));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashbytes.Length; ++i)
                {
                    builder.Append(hashbytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static byte[] HmacSHA256(byte[] key, byte[] msg)
        {
            using (HMACSHA256 mac = new HMACSHA256(key))
            {
                return mac.ComputeHash(msg);
            }
        }

        private static string BuildParamStr(SortedDictionary<string, string> requestParams, string requestMethod = "GET")
        {
            string retStr = "";
            foreach (string key in requestParams.Keys)
            {
                retStr += string.Format("{0}={1}&", key, requestParams[key]);
            }
            return retStr.TrimEnd('&');
        }

        public static string MakeSignPlainText(SortedDictionary<string, string> requestParams, string requestMethod, string requestHost, string requestPath)
        {
            string retStr = "";
            retStr += requestMethod;
            retStr += requestHost;
            retStr += requestPath;
            retStr += "?";
            retStr += BuildParamStr(requestParams);
            return retStr;
        }
    }
}
