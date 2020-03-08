using Core;
using Resillience.ServiceDiscovery.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.ServiceDiscovery
{
    public static class ServiceProviderExtension
    {
        public static IServiceDiscoveryBuilder CreateServiceBuilder(this IServiceDiscoveryProvider serviceProvider, Action<IServiceDiscoveryBuilder> config)
        {

            var builder = new ServiceDiscoveryBuilder(serviceProvider);
            config(builder);
            return builder;
        }
    }
}
