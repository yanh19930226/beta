using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Consul;
using Resilience.Http;
using Resilience.Identity;

namespace Contact.Api.Service
{
    public class UserService : IUserService
    {

        private readonly string _userServiceUrl;
        private IHttpClient _httpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpClient httpClient, IOptions<ServiceDisvoveryOptions> serviceOptions, IDnsQuery dnsQuery)
        {
            _httpClient = httpClient;
#warning UserApi为指定的调用的服务的名称
            var address = dnsQuery.ResolveService("service.consul", "UserApi");
            var addrssList = address.First().AddressList;
            var host = addrssList.Any() ? addrssList.First().ToString() : address.First().HostName;
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
        }
        public async Task<UserIdentity> GetBaseUserInfoAsync(int userId)
        {
            try
            {
#warning 由于请求的封装Get存在问题,此处没有使用ResilientHttp和Polly
                var client = new HttpClient();
                var response =  client.GetStringAsync(_userServiceUrl + "/api/user/getuserinfo/" + userId).Result;
                //var response = await _httpClient.GetStringAsync(_userServiceUrl + "/api/user/getuserinfo/" + userId);
                if (!string.IsNullOrEmpty(response))
                {
                    var userInfo = JsonConvert.DeserializeObject<UserIdentity>(response);
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return null;
        }
    }
}
