using Resilience.Zeus.Domain.Core.Models;
using ServiceB.DomainEvents.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Models
{
    public class Post : Entity
    {
        public Post()
        {
            this.AddDomainEvent(new CreatePostDomainEvent(this));
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public long BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual List<PostTag> PostTags { get; set; }
    }
}
