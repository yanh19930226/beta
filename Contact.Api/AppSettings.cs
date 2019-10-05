using Contact.Api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api
{
    public class AppSettings
    {
       public string MongoContactConnectionString { get; set; }
       public string MongoContactDatabase { get; set; }
    }
}
