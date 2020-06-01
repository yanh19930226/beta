using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Resillience.Hangfire
{
    public  class JobManager:IJobManager
    {
        private   List<Expression<Action>> actions { get; set; }

        public  void AddJob(Expression<Action> action)
        {
            actions.Add(action);
        }

        public void Run()
        {
            foreach (var item in actions)
            {
                RecurringJob.AddOrUpdate("定时任务测试", item, Cron.Minutely());
            }
        }
    }
}
