using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.SmsService.TencentSms.SDK
{
    class TencentSmsClient
    {

        private readonly string _accessKeyId;
        private readonly string _accessSecret;
        private readonly Endpoint _endpoint;
        private readonly ProtocolType _protocolType;
        //private readonly FormatType _FormatType;
        private const string SEPARATOR = "&";
        public TencentSmsClient(string accessKeyId, string accessSecret, ProtocolType protocolType, Endpoint endpoint)
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

        #endregion

        public async Task<T> RequestAsync<T>(BaseRequest<T> request)
        {

            return await $"{url}".GetJsonAsync<T>();
        }
    }
}
