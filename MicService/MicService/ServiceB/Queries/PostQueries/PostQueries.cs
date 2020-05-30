using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resilience.Zeus.Domain.Interfaces;
using ServiceB.DTO.Post;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Queries.PostQueries
{
    public class PostQueries : IPostQueries
    {
        private readonly IMapper _mapper;
        public readonly IRepository<Post> _postRepository;
        public PostQueries(IRepository<Post> postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// 列表数据不分页
        /// </summary>
        /// <returns></returns>
        public IQueryable<Post> GetAll()
        {
            return _postRepository.GetAll().AsNoTracking();
        }
        /// <summary>
        /// 列表数据分页
        /// </summary>
        /// <returns></returns>
        public IQueryable<Post> GetPage()
        {
            int totalRecord;
            var postpage = _postRepository.GetByPage(1, 2, out totalRecord, p => true, true, p => p.Id).AsNoTracking();
            return postpage;
        }
        /// <summary>
        /// 列表关联分页
        /// </summary>
        /// <returns></returns>
        public IQueryable<PostDTO> GetPageJoin()
        {
            #region 关闭LazyLoad使用贪婪加载查询关联表

            //关闭LazyLoad使用贪婪加载查询关联表
            //var include = _postRepository.GetAll().Include(p => p.Blog);

            #endregion

            int totalRecord;
            var postpage = _postRepository.GetByPage(1, 2, out totalRecord, p => true, true, p => p.Id).Include(p=>p.Blog).Select(p => new PostDTO
            {
                PostName = p.Title,
                BlogName = p.Blog.Name
            }).AsNoTracking(); 


            return postpage;
        }
    }
}
