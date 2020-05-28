using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Auth
{
    public class JWT
    {
        public static string Domain { get; set; }
        public static string SecurityKey { get; set; }
        public  static int Expires { get; set; }
    }
}
