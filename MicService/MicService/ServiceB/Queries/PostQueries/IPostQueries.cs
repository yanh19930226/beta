using Resillience.ResillienceApiResult;
using ServiceB.DTO.Post;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Queries.PostQueries
{
    public interface IPostQueries
    {
        ApiResult<IQueryable<Post>> GetAll();
        ApiResult<IQueryable<Post>> GetPage(PostPageRequestDTO req);

        IQueryable<PostDTO> GetPageJoin(PostPageRequestDTO req);
    }
}
