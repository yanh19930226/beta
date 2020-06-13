using Newtonsoft.Json;
using Resillience.SmsService.TencentSms.SDK.Common;
using Resillience.SmsService.TencentSms.SDK.Common.Exceptions;
using Resillience.SmsService.TencentSms.SDK.Common.Http;
using Resillience.SmsService.TencentSms.SDK.Common.Profile;
using Resillience.SmsService.TencentSms.SDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.SmsService.TencentSms.SDK
{
    public class TencentSmsClient : AbstractClient
    {
        private const string endpoint = "sms.tencentcloudapi.com";
        private const string version = "2019-07-11";

        /// <summary>
        /// Client constructor.
        /// </summary>
        /// <param name="credential">Credentials.</param>
        /// <param name="region">Region name, such as "ap-guangzhou".</param>
        public TencentSmsClient(Credential credential, string region)
            : this(credential, region, new ClientProfile())
        {

        }
        /// <summary>
        /// Client Constructor.
        /// </summary>
        /// <param name="credential">Credentials.</param>
        /// <param name="region">Region name, such as "ap-guangzhou".</param>
        /// <param name="profile">Client profiles.</param>
        public TencentSmsClient(Credential credential, string region, ClientProfile profile)
            : base(endpoint, version, credential, region, profile)
        {

        }

        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<SendSmsResponse> SendSms(SendSmsRequest req)
        {
            JsonResponseModel<SendSmsResponse> rsp = null;
            try
            {
                var strResp = await this.InternalRequest(req, "SendSms");
                rsp = JsonConvert.DeserializeObject<JsonResponseModel<SendSmsResponse>>(strResp);
            }
            catch (JsonSerializationException e)
            {
                throw new TencentCloudSDKException(e.Message);
            }
            return rsp.Response;
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public SendSmsResponse SendSmsSync(SendSmsRequest req)
        {
            JsonResponseModel<SendSmsResponse> rsp = null;
            try
            {
                var strResp = this.InternalRequestSync(req, "SendSms");
                rsp = JsonConvert.DeserializeObject<JsonResponseModel<SendSmsResponse>>(strResp);
            }
            catch (JsonSerializationException e)
            {
                throw new TencentCloudSDKException(e.Message);
            }
            return rsp.Response;
        } 
        #endregion

    }
}
