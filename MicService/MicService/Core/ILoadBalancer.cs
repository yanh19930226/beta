using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface ILoadBalancer
    {
        string Resolve(IList<string> services);
    }
}
