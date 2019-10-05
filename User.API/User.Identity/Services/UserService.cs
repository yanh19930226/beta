using DnsClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using User.Identity.Dto;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        //private readonly string _userServiceUrl = "http://localhost:5000";
        private readonly string _userServiceUrl;
        private HttpClient _httpClient;
        public UserService(HttpClient httpClient, IOptions<ServiceDisvoveryOptions> serviceOptions,IDnsQuery dnsQuery)
        {
            _httpClient = httpClient;
            var address=dnsQuery.ResolveService("service.consul", serviceOptions.Value.UserServiceName);
            var addrssList = address.First().AddressList;
            var host = addrssList.Any()? addrssList.First().ToString(): address.First().HostName;
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
        }
        public async Task<int> CheckOrCreate(string phone)
        {
            //var content = new FormUrlEncodedContent(new Dictionary<string,string>(){{ "phone", phone}});
            var content = new MultipartFormDataContent();
            //添加字符串参数，参数名为qq
            content.Add(new StringContent(phone), "phone");

            //var form = new Dictionary<string, string>() { { "phone", phone } };
            //var content = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync(_userServiceUrl + "/api/user/check-or-create", content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var userId = await response.Content.ReadAsStringAsync();
                int.TryParse(userId, out int intuserId);
                return intuserId;
            }
            return 0;
        }
    }
}
