using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Exceptions
{
    public class ResillienceException: Exception
    {
        public ResillienceException()
        {

        }
        public ResillienceException(string message) : base(message)
        {

        }
        public ResillienceException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
