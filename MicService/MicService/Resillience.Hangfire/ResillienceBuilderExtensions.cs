using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Resillience.Hangfire
{
    public static class ResillienceBuilderExtensions
    {
        public static ResillienceBuilder AddHangfire(this ResillienceBuilder builder, IConfiguration configuration = null)
        {
            configuration = (configuration ?? builder.Services.BuildServiceProvider().GetService<IConfiguration>());
            IServiceCollection services = builder.Services;
            services.AddSingleton<IJobManager,JobManager>();
            services.AddHangfire(config =>
            {
                var str = configuration.GetSection("Resillience:Hangfire:Connection").Value;
                config.UseStorage(
                    new MySqlStorage(configuration.GetSection("Resillience:Hangfire:Connection").Value,
                    new MySqlStorageOptions
                    {
                        TablePrefix ="yanh_hangfire"
                    }));
            });
            return builder;
        }
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app, Action<IJobManager> job=null)
        {
            IConfiguration configuration = app.ApplicationServices.GetService<IConfiguration>();
            app.UseHangfireServer();
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                IgnoreAntiforgeryToken = true,
                AppPath = "#",//返回时跳转的地址
                DisplayStorageConnectionString = false,//是否显示数据库连接信息
                IsReadOnlyFunc = Context =>
                {
                    return true;
                },
                Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,//是否启用ssl验证，即https
                    SslRedirect = false,
                    LoginCaseSensitive = true,
                    Users = new []
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = "read",
                            PasswordClear = "only"
                        },
                        new BasicAuthAuthorizationUser
                        {
                            Login = "yanh",
                            PasswordClear = "123"
                        },
                        new BasicAuthAuthorizationUser
                        {
                            Login = "guest",
                            PasswordClear = "123@123"
                        }
                    }
                })
                },
                DashboardTitle = "任务调度中心"
            });

            #region Todo 传入委托Job调用Job执行定时任务

            //var jobManager = app.ApplicationServices.GetRequiredService<IJobManager>();

            //var a=job.GetInvocationList();
            //foreach (var item in job.GetInvocationList())
            //{
            //    item.Method.Invoke()
            //}
            //job.Invoke(jobManager);
            //jobManager.Run();

            //RecurringJob.AddOrUpdate("定时任务测试",, Cron.Minutely());

            #endregion

            return app;
        }
    }
}
