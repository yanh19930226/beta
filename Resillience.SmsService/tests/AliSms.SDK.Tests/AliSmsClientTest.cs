using Resillience.SmsService.AliSms.SDK;
using Resillience.SmsService.AliSms.SDK.Models;
using Resillience.SmsService.AliSms.SDK.Models.SendSms;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AliSms.SDK.Tests
{
    public class AliSmsClientTest
    {

        private AliSmsClient _client;

        public AliSmsClientTest()
        {
            _client = new AliSmsClient("LTAI4Fzjg1d4vtpS3uKHUZ8B", "tpjRAwo98TKUih7T2UGlCWmPp7Il3g",ProtocolType.HTTP,Endpoint.Send);
        }
        /// <summary>
        /// ∑¢ÀÕ∂Ã–≈(≤‚ ‘)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendSms()
        {
            var result = await _client.RequestAsync(new SendSmsRequest()
            {
                PhoneNumbers = "18650482503"
            });

            Assert.Equal("OK", result.Code);
        }

    }
}
