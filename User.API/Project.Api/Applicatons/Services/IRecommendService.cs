using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applicatons.Services
{
    public interface IRecommendService
    {
        Task<bool> IsRecommendProject(int projectId,int userId);
    }
}
