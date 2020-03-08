using Resillience.ServiceDiscovery;
using Resillience.ServiceDiscovery.LoadBalancer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MicService.Tests
{
    public class Test
    {
        [Fact]
        public async Task GetServiceDiscovery()
        {
            var serviceDiscoveryProvider = new ConsulServiceProvider(new Uri("http://127.0.0.1:8500"));

            #region 获取服务节点列表
            //var serviceAList = await serviceDiscoveryProvider.GetServicesAsync("ServiceA");
            //return serviceAList; 
            #endregion

            var serviceA = serviceDiscoveryProvider.CreateServiceBuilder(builder =>
            {
                builder.ServiceName = "ServiceA";
                // 指定负载均衡器
                builder.LoadBalancer = TypeLoadBalancer.RoundRobin;
                // 指定Uri方案
                builder.UriScheme = Uri.UriSchemeHttp;
            });
            var httpClient = new HttpClient();
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"-------------第{i}次请求-------------");
                try
                {
                    var uri = serviceA.BuildAsync("health").Result;
                    System.Diagnostics.Debug.WriteLine($"{DateTime.Now} - 正在调用：{uri}");
                    var content = httpClient.GetStringAsync(uri).Result;
                    System.Diagnostics.Debug.WriteLine($"调用结果：{content}");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"调用异常：{e.GetType()}");
                }
                Task.Delay(1000).Wait();
            }
        }
    }
}
