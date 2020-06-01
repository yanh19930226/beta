using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Auth
{
    public class GitHub
    {
        public   int UserId { get; set; }
        public  string Client_ID { get; set; }
        public  string Client_Secret { get; set; }
        public  string Redirect_Uri { get; set; }
        public  string ApplicationName { get; set; }
    }
}
