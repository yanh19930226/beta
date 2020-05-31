using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.DTO.Post
{
    public class PostDTO
    {
        public string PostName { get; set; }
        public string BlogName { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
