using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Queries
{
    public interface ITestQueries
    {
        IQueryable<TestModel> GetAll();
    }
}
