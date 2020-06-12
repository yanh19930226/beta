using Resillience.SmsService.TencentSms.SDK;
using Resillience.SmsService.TencentSms.SDK.Common;
using Resillience.SmsService.TencentSms.SDK.Common.Profile;
using Resillience.SmsService.TencentSms.SDK.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TencentSms.SDK.Tests
{
    public class TencentSmsClientTest
    {
        private TencentSmsClient _client;
        Credential cred = new Credential
        {
            SecretId = "AKIDIK5ZLHnJOJyNTcB8UZNg9KD7XgX7nIxM",
            SecretKey = "EEPLyLQEkxKOBpFNDm3hWgZAVXHDZeS1"
        };
        ClientProfile clientProfile = new ClientProfile {
            SignMethod = ClientProfile.SIGN_TC3SHA256,
            HttpProfile= new HttpProfile
            {
                ReqMethod = "GET",
                Timeout = 10,
                Endpoint = "sms.tencentcloudapi.com",
                WebProxy = Environment.GetEnvironmentVariable("HTTPS_PROXY")
            }
        };
        public TencentSmsClientTest()
        {
            _client = new TencentSmsClient(cred, "ap-guangzhou", clientProfile);
        }
        /// <summary>
        /// ·¢ËÍ¶ÌÐÅ(²âÊÔ)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendSms()
        {
            var result = await _client.SendSms(new SendSmsRequest()
            {
                PhoneNumberSet = new string []{ "18650482503"},
                TemplateID = "SMS_192821653",
                //SignName = "ÂþÍæ",
                //TemplateParam = "{'code':'2345'}"
            }); 
            //Assert.Equal("OK", result.Code);
        }
    }
}
