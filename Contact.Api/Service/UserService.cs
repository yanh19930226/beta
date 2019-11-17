using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Dto;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience.Http;

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
            var address = dnsQuery.ResolveService("service.consul", serviceOptions.Value.UserServiceName);
            var addrssList = address.First().AddressList;
            var host = addrssList.Any() ? addrssList.First().ToString() : address.First().HostName;
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
        }
        public async Task<UserIdentity> GetBaseUserInfoAsync(int userId)
        {
            //try
            //{
            //    var response = await _httpClient.PostAsync(_userServiceUrl + "/api/user/check-or-create", form);
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        var result = await response.Content.ReadAsStringAsync();
            //        return JsonConvert.DeserializeObject<UserIdentity>(result);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("在重试之后失败");
            //    throw new Exception(ex.Message);
            //}
            return null;
        }
    }
}
