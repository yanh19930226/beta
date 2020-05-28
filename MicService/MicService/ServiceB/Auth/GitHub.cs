using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Auth
{
    public class GitHub
    {
        public  static int UserId { get; set; }
        public static string Client_ID { get; set; }
        public static string Client_Secret { get; set; }
        public static string Redirect_Uri { get; set; }
        public static string ApplicationName { get; set; }
    }
}
