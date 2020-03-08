using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IServiceDiscoveryProvider
    {
        Task<IList<string>> GetServicesAsync(string serviceName);
    }
}
