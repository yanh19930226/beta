using Resilience.Zeus.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Models
{
    public class PostTag : Entity
    {
        public long PostId { get; set; }
        public virtual Post Post { get; set; }
        public long TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
