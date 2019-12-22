using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.AggregatesModel
{
    public interface IProjectRepository : IRepository<Project>
    {

        //自定义方法接口
        //Task<Project> GetAsync(int id);

        //Project Add(Project project);

        //Project Update(Project project);
    }
}
