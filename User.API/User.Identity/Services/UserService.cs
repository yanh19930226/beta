﻿using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Consul;
using Resilience.Http;
using Resilience.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        //private readonly string _userServiceUrl = "http://localhost:5000";
        private readonly string _userServiceUrl;
        private IHttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        public UserService(IHttpClient httpClient, IOptions<ServiceDisvoveryOptions> serviceOptions,IDnsQuery dnsQuery)
        {
            _httpClient = httpClient;
            var address=dnsQuery.ResolveService("service.consul", "UserApi");
            var addrssList = address.First().AddressList;
            var host = addrssList.Any()? addrssList.First().ToString(): address.First().HostName;
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
        }
        public async Task<UserIdentity> CheckOrCreate(string phone)
        {
            var form = new Dictionary<string, string>() { { "phone", phone } };
            try
            {
                var response = await _httpClient.PostAsync(_userServiceUrl + "/api/user/check-or-create", form);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserIdentity>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("在重试之后失败");
                throw new Exception(ex.Message);
            }
            return null;
        }
    }
}
