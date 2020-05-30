using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.DTO.Post
{
    /// <summary>
    /// Post创建DTO
    /// </summary>
    public class CreatePostDTO
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public long BlogId { get; set; }
    }
}
