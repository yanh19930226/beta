using Resilience.Zeus.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Models
{
    public class Tag : Entity
    {
        public string Name { get; set; }
        public virtual List<PostTag> PostTags { get; set; }
    }
}
