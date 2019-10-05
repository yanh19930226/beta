using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API
{
    public class UserOprationException: Exception
    {
        public UserOprationException()
        {

        }
        public UserOprationException(string message):base(message)
        {

        }
        public UserOprationException(string message,Exception ex) : base(message,ex)
        {

        }
    }
}
