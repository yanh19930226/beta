using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.DTO.Post
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PostPageRequestDTO
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Post名字搜索
        /// </summary>
        public string PostNameSearch { get; set; }
        /// <summary>
        /// Blog名字搜索
        /// </summary>
        public string BlogNameSearch { get; set; }
    }
}
