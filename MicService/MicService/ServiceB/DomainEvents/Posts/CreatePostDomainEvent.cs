using Resilience.Zeus.Domain.Core.Events;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.DomainEvents.Posts
{
    public class CreatePostDomainEvent : Event
    {
        public CreatePostDomainEvent(Post post)
        {
            Post = post;
        }

        public Post Post { get; set; }
    }
}
