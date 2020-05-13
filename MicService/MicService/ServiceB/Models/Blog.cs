using Resilience.Zeus.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Models
{
    public class Blog : Entity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}
