using System;
using System.Collections.Generic;
using System.Text;

namespace Resilience.Consul
{
    public class ConsulOptions
    {
        public string HttpEndpoint { get; set; }
        public DnsEndpoint DnsEndpoint { get; set; }
    }
}
