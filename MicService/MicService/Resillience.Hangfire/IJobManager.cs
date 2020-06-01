using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Resillience.Hangfire
{
    public interface IJobManager
    {
        void AddJob(Expression<Action> action);
        void Run();
    }
}
