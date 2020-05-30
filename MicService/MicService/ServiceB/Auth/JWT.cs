using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Auth
{
    public class JWT
    {
        public  string Domain { get; set; }
        public  string SecurityKey { get; set; }
        public   int Expires { get; set; }
    }
}
