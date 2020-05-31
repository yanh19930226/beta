using Resilience.Zeus.Domain.Core.Commands;
using ServiceB.DTO.Post;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Commands.Posts
{
    public class UpdatePostCommand : Command
    {
        public UpdatePostCommand(UpdatePostDTO post)
        {
            Post = post;
        }
        public UpdatePostDTO Post { get; set; }
    }
}
