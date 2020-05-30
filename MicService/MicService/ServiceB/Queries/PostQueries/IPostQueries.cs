using ServiceB.DTO.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Queries.PostQueries
{
    public interface IPostQueries
    {
        IQueryable<Models.Post> GetAll();


        IQueryable<Models.Post> GetPage();

        IQueryable<PostDTO> GetPageJoin();
    }
}
