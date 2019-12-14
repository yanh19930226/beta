using System;
using System.Collections.Generic;
using System.Text;

namespace Resilience.Consul
{
    public class ServiceDisvoveryOptions
    {
        public string ServiceName { get; set; }
        public ConsulOptions Consul { get; set; }
    }
}
