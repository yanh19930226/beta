using Flurl.Http;
using Newtonsoft.Json;
using Resillience.SmsService.AliSms.SDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Resillience.SmsService.AliSms.SDK
{
    public class AliSmsClient
    {
        private readonly string _accessKeyId;
        private readonly string _accessSecret;
        private readonly Endpoint _endpoint;
        private readonly ProtocolType _protocolType;
        //private readonly FormatType _FormatType;
        private const string SEPARATOR = "&";
        public AliSmsClient(string accessKeyId,string accessSecret, ProtocolType protocolType, Endpoint endpoint)
        {
            _accessKeyId = accessKeyId;
            _accessSecret = accessSecret;
            _protocolType = protocolType;
            _endpoint = endpoint;
        }

        #region Utilties
        private Dictionary<string, string> ToDictionary(object obj)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();

                if (m != null && m.IsPublic)
                {
                    // 进行判NULL处理 
                    if (m.Invoke(obj, new object[] { }) != null)
                    {
                        if (m.ReturnType == typeof(string))
                        {
                            map.Add(p.Name, (string)m.Invoke(obj, new object[] { })); // 向字典添加元素
                        }
                        else
                        {
                            map.Add(p.Name, JsonConvert.SerializeObject(m.Invoke(obj, new object[] { }))); // 向字典添加元素
                        }
                    }
                }
            }
            return map;
        }

        private string MD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            string result = BitConverter.ToString(md5.ComputeHash(bytes));
            return result.Replace("-", "");
        }
        /// <summary>
        /// 请求地址
        /// </summary>
        /// <returns></returns>
        private string GetEndpoint()
        {
            switch (_endpoint)
            {
                case Endpoint.Send:
                    return "dysmsapi.aliyuncs.com";
                case Endpoint.Receive1:
                    return "dybaseapi.aliyuncs.com";
                default:
                    return "dysmsapi.aliyuncs.com";
            }
        }
        /// <summary>
        /// 请求参数排序
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static IDictionary<string, string> SortDictionary(Dictionary<string, string> dic)
        {
            IDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dic, StringComparer.Ordinal);
            return sortedDictionary;
        }
        /// <summary>
        /// 构造键值参数字符串UTF-8
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string ConcatQueryString(Dictionary<string, string> parameters)
        {
            if (null == parameters)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();

            foreach (var entry in parameters)
            {
                string key = entry.Key;
                string val = entry.Value;

                sb.Append(AcsURLEncoder.Encode(key));
                if (val != null)
                {
                    sb.Append("=").Append(AcsURLEncoder.Encode(val));
                }
                sb.Append("&");
            }

            int strIndex = sb.Length;
            if (parameters.Count > 0)
                sb.Remove(strIndex - 1, 1);
            return sb.ToString();
        }
        /// <summary>
        /// 待签字符串加密方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="accessSecret"></param>
        /// <returns></returns>
        public string SignString(string source, string accessSecret)
        {
            using (var algorithm = KeyedHashAlgorithm.Create("HMACSHA1"))
            {
                algorithm.Key = Encoding.UTF8.GetBytes(accessSecret.ToCharArray());
                return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(source.ToCharArray())));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="queries"></param>
        /// <returns></returns>
        public string ComposeUrl(string endpoint, Dictionary<string, string> queries)
        {
            Dictionary<string, string> mapQueries =queries;
            StringBuilder urlBuilder = new StringBuilder("");
            urlBuilder.Append(_protocolType);
            urlBuilder.Append("://").Append(endpoint);
            if (-1 == urlBuilder.ToString().IndexOf('?'))
            {
                urlBuilder.Append("?");
            }
            else if (!urlBuilder.ToString().EndsWith("?"))
            {
                urlBuilder.Append("&");
            }
            string query = ConcatQueryString(mapQueries);
            string url = urlBuilder.Append(query).ToString();
            if (url.EndsWith("?") || url.EndsWith("&"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            return url;
        }


        public string ComposeStringToSign(MethodType? method, Dictionary<string, string> queries)
        {
            var sortedDictionary = SortDictionary(queries);

            StringBuilder canonicalizedQueryString = new StringBuilder();
            foreach (var p in sortedDictionary)
            {
                canonicalizedQueryString.Append("&")
                .Append(AcsURLEncoder.PercentEncode(p.Key)).Append("=")
                .Append(AcsURLEncoder.PercentEncode(p.Value));
            }

            StringBuilder stringToSign = new StringBuilder();
            stringToSign.Append(method.ToString());
            stringToSign.Append(SEPARATOR);
            stringToSign.Append(AcsURLEncoder.PercentEncode("/"));
            stringToSign.Append(SEPARATOR);
            stringToSign.Append(AcsURLEncoder.PercentEncode(
                    canonicalizedQueryString.ToString().Substring(1)));

            return stringToSign.ToString();
        }

        #endregion

        public async Task<T> RequestAsync<T>(BaseRequest<T> request)
        {
            var original = ToDictionary(request);
            var stringToSign=this.ComposeStringToSign(MethodType.GET, original);
            var signature = SignString(stringToSign, _accessSecret);
            original.Add("Signature", signature);
            var url=ComposeUrl(GetEndpoint(), original);

            return await $"{url}".GetJsonAsync<T>();
        }
    }

    public class AcsURLEncoder
    {
        private const string ENCODING_UTF8 = "UTF-8";

        public static string Encode(string value)
        {
            return HttpUtility.UrlEncode(value, Encoding.UTF8);
        }

        public static string PercentEncode(string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            byte[] bytes = Encoding.GetEncoding(ENCODING_UTF8).GetBytes(value);
            foreach (char c in bytes)
            {
                if (text.IndexOf(c) >= 0)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append("%").Append(
                        string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
                }
            }
            return stringBuilder.ToString();
        }
    }
}
