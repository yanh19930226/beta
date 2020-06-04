using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Resillience.Hangfire
{
    public  class JobManager:IJobManager
    {
        private List<Expression<Action>> actions = new List<Expression<Action>>();


        private readonly IList<Func<Expression<Action>, Expression<Action>>> _components = new List<Func<Expression<Action>, Expression<Action>>>();

        public  void AddJob(Expression<Action> action)
        {
            if (action!=null)
            {
                this.actions.Add(action);
            }
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
