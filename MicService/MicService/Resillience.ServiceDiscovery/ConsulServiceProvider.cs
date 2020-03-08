using Consul;
using Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resillience.ServiceDiscovery
{
    public class ConsulServiceProvider : IServiceDiscoveryProvider
    {
        private readonly ConsulClient _consuleClient;

        public ConsulServiceProvider(Uri uri)
        {
            _consuleClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = uri;
            });
        }

        public async Task<IList<string>> GetServicesAsync(string serviceName)
        {
            // Health 当前consul里已注册的服务，健康检查的信息也拿过来
            // HTTP API
            var queryResult = await _consuleClient.Health.Service(serviceName, "", true);
            var result = new List<string>();
            foreach (var service in queryResult.Response)
            {
                result.Add(service.Service.Address + ":" + service.Service.Port);
            }
            return result;
        }
    }
}
