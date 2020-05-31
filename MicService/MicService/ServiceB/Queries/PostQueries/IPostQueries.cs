using Resillience.Util.ResillienceResult;
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
        ResillienceResult<IQueryable<Post>> GetAll();
        PageResult<IQueryable<Post>> GetPage(PostPageRequestDTO req);
        PageResult<IQueryable<PostDTO>> GetPageJoin(PostPageRequestDTO req);
    }
}
