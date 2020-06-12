using Newtonsoft.Json;
using Resillience.SmsService.TencentSms.SDK.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.TencentSms.SDK.Models
{
    public class SendSmsRequest : AbstractModel
    {

        /// <summary>
        /// 下发手机号码，采用 e.164 标准，格式为+[国家或地区码][手机号]，单次请求最多支持200个手机号且要求全为境内手机号或全为境外手机号。
        /// 例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号。
        /// </summary>
        [JsonProperty("PhoneNumberSet")]
        public string[] PhoneNumberSet { get; set; }

        /// <summary>
        /// 模板 ID，必须填写已审核通过的模板 ID。模板ID可登录 [短信控制台](https://console.cloud.tencent.com/smsv2) 查看。
        /// </summary>
        [JsonProperty("TemplateID")]
        public string TemplateID { get; set; }

        /// <summary>
        /// 短信SdkAppid在 [短信控制台](https://console.cloud.tencent.com/smsv2)  添加应用后生成的实际SdkAppid，示例如1400006666。
        /// </summary>
        [JsonProperty("SmsSdkAppid")]
        public string SmsSdkAppid { get; set; }

        /// <summary>
        /// 短信签名内容，使用 UTF-8 编码，必须填写已审核通过的签名，签名信息可登录 [短信控制台](https://console.cloud.tencent.com/smsv2)  查看。注：国内短信为必填参数。
        /// </summary>
        [JsonProperty("Sign")]
        public string Sign { get; set; }

        /// <summary>
        /// 模板参数，若无模板参数，则设置为空。
        /// </summary>
        [JsonProperty("TemplateParamSet")]
        public string[] TemplateParamSet { get; set; }

        /// <summary>
        /// 短信码号扩展号，默认未开通，如需开通请联系 [sms helper](https://cloud.tencent.com/document/product/382/3773)。
        /// </summary>
        [JsonProperty("ExtendCode")]
        public string ExtendCode { get; set; }

        /// <summary>
        /// 用户的 session 内容，可以携带用户侧 ID 等上下文信息，server 会原样返回。
        /// </summary>
        [JsonProperty("SessionContext")]
        public string SessionContext { get; set; }

        /// <summary>
        /// 国际/港澳台短信 senderid，国内短信填空，默认未开通，如需开通请联系 [sms helper](https://cloud.tencent.com/document/product/382/3773)。
        /// </summary>
        [JsonProperty("SenderId")]
        public string SenderId { get; set; }

        /// <summary>
        /// For internal usage only. DO NOT USE IT.
        /// </summary>
        internal override void ToMap(Dictionary<string, string> map, string prefix)
        {
            this.SetParamArraySimple(map, prefix + "PhoneNumberSet.", this.PhoneNumberSet);
            this.SetParamSimple(map, prefix + "TemplateID", this.TemplateID);
            this.SetParamSimple(map, prefix + "SmsSdkAppid", this.SmsSdkAppid);
            this.SetParamSimple(map, prefix + "Sign", this.Sign);
            this.SetParamArraySimple(map, prefix + "TemplateParamSet.", this.TemplateParamSet);
            this.SetParamSimple(map, prefix + "ExtendCode", this.ExtendCode);
            this.SetParamSimple(map, prefix + "SessionContext", this.SessionContext);
            this.SetParamSimple(map, prefix + "SenderId", this.SenderId);
        }
    }

    public class SendSmsResponse : AbstractModel
    {
        /// <summary>
        /// 短信发送状态。
        /// </summary>
        [JsonProperty("SendStatusSet")]
        public SendStatus[] SendStatusSet { get; set; }

        /// <summary>
        /// 唯一请求 ID，每次请求都会返回。定位问题时需要提供该次请求的 RequestId。
        /// </summary>
        [JsonProperty("RequestId")]
        public string RequestId { get; set; }


        /// <summary>
        /// For internal usage only. DO NOT USE IT.
        /// </summary>
        internal override void ToMap(Dictionary<string, string> map, string prefix)
        {
            this.SetParamArrayObj(map, prefix + "SendStatusSet.", this.SendStatusSet);
            this.SetParamSimple(map, prefix + "RequestId", this.RequestId);
        }
    }

    public class SendStatus : AbstractModel
    {
        /// <summary>
        /// 发送流水号。
        /// </summary>
        [JsonProperty("SerialNo")]
        public string SerialNo { get; set; }

        /// <summary>
        /// 手机号码,e.164标准，+[国家或地区码][手机号] ，示例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号。
        /// </summary>
        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 计费条数，计费规则请查询 [计费策略](https://cloud.tencent.com/document/product/382/36135)。
        /// </summary>
        [JsonProperty("Fee")]
        public ulong? Fee { get; set; }

        /// <summary>
        /// 用户Session内容。
        /// </summary>
        [JsonProperty("SessionContext")]
        public string SessionContext { get; set; }

        /// <summary>
        /// 短信请求错误码，具体含义请参考错误码。
        /// </summary>
        [JsonProperty("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 短信请求错误码描述。
        /// </summary>
        [JsonProperty("Message")]
        public string Message { get; set; }


        /// <summary>
        /// For internal usage only. DO NOT USE IT.
        /// </summary>
        internal override void ToMap(Dictionary<string, string> map, string prefix)
        {
            this.SetParamSimple(map, prefix + "SerialNo", this.SerialNo);
            this.SetParamSimple(map, prefix + "PhoneNumber", this.PhoneNumber);
            this.SetParamSimple(map, prefix + "Fee", this.Fee);
            this.SetParamSimple(map, prefix + "SessionContext", this.SessionContext);
            this.SetParamSimple(map, prefix + "Code", this.Code);
            this.SetParamSimple(map, prefix + "Message", this.Message);
        }
    }
}
