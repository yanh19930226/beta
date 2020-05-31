using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resilience.Zeus.Domain.Interfaces;
using Resillience.Extensions;
using Resillience.Util;
using Resillience.Util.ResillienceResult;
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
        public ResillienceResult<IQueryable<Post>> GetAll()
        {
            var result = new ResillienceResult<IQueryable<Post>>();
            var post=_postRepository.GetAll().AsNoTracking();
            if (post == null)
            {
                result.IsFailed("数据不存在");
                return result;
            }
            result.IsSuccess(post);
            return result;
        }
        /// <summary>
        /// 列表数据分页
        /// </summary>
        /// <returns></returns>
        public PageResult<IQueryable<Post>> GetPage(PostPageRequestDTO req)
        {
            var expression = LinqExtensions.True<Post>();

            if (!string.IsNullOrEmpty(req.PostNameSearch))
            {
                expression = expression.And(t => t.Title.Contains(req.PostNameSearch));
            }
            var postpage = _postRepository.GetAll().ToPage(req.PageIndex, req.PageSize, expression, p => p.Id, true);
            return postpage;
        }
        /// <summary>
        /// 列表关联分页
        /// </summary>
        /// <returns></returns>
        public PageResult<IQueryable<PostDTO>> GetPageJoin(PostPageRequestDTO req)
        {
            #region 关闭LazyLoad使用贪婪加载查询关联表

            //关闭LazyLoad使用贪婪加载查询关联表
            //var include = _postRepository.GetAll().Include(p => p.Blog);

            #endregion

            #region Expression Tree MultiOption Search

            var expression = LinqExtensions.True<Post>();

            if (!string.IsNullOrEmpty(req.PostNameSearch))
            {
                expression = expression.And(t => t.Title.Contains(req.PostNameSearch));
            }
            if (!string.IsNullOrEmpty(req.BlogNameSearch))
            {
                expression = expression.And(t => t.Blog.Name.Contains(req.BlogNameSearch));
            }

            #endregion

            #region Expression Tree MultiOption Order

            #endregion

            int totalcount;
            var postpage = _postRepository.GetByPage(req.PageIndex, req.PageSize, expression, p => p.Id, true, out totalcount).Include(p => p.Blog).Select(p => new PostDTO
            {
                PostName = p.Title,
                BlogName = p.Blog.Name,
                //Tags=p.PostTags.
            }).ToPage(totalcount);

            return postpage;
        }
    }
}
