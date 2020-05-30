using Resilience.Zeus.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Commands.Posts
{
    public class CreatePostCommand : Command
    {
        public CreatePostCommand(string title, string content, long blogId)
        {
            Title = title;
            Content = content;
            BlogId = blogId;
        }
        public string Title { get; set; }
        public string Content { get; set; }
        public long BlogId { get; set; }
    }
}
