using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Consul;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Resilience.Consul;
using Project.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Project.Domain.AggregatesModel;
using Project.Api.Applicatons.Queries;
using Project.Api.Applicatons.Services;
using Project.Infrastructure.Repositories;

namespace Project.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region MediatR
            services.AddMediatR();
            #endregion

            #region 接口
            services.AddScoped<IRecommendService, TestRecommendService>()
                   .AddScoped<IProjectQueries, ProjectQueries>(sp =>
                   {
                       return new ProjectQueries(Configuration.GetConnectionString("MySqlConnection"));
                   })
                    .AddScoped<IProjectRepository, ProjectRepository>(sp =>
                    {
                        var projectContext = sp.GetRequiredService<ProjectContext>();
                        return new ProjectRepository(projectContext);
                    });
            #endregion

            #region MySql

            services.AddDbContext<ProjectContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"), b => b.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));
            });
            #endregion

            #region 认证授权
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5001";
                    options.Audience = "project_api";
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });
            #endregion

            #region consul相关配置
            //读取consul相关配置
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //DnsQuery
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            #endregion

            #region Swagger配置
            //Swagger配置
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Project.Api", new Info { Title = "Project.Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region CAP
            services.AddCap(options =>
            {
                options.UseMySql("server=localhost;port=3306;database=beta_contact;userid=yanh;password=123")
               .UseRabbitMQ("localhost")
               .UseDashboard();

                options.UseDiscovery(opt =>
                {
                    opt.DiscoveryServerHostName = "localhost";
                    opt.DiscoveryServerPort = 8500;
                    opt.CurrentNodeHostName = "localhost";
                    opt.CurrentNodePort = 5802;
                    opt.NodeId = "3";
                    opt.NodeName = "CAP No.3 Node";
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime,
            IOptions<ServiceDisvoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 认证授权
            //认证授权
            app.UseAuthentication();
            #endregion

            #region Swagger配置
            //Swagger配置
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{documentName}/swagger.json";
            });
            #endregion

            #region Consul
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/Project.Api/swagger.json", "Project.Api"); });
            //启动的时候注册服务
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, serviceOptions, consul);
            });
            //停止的时候移除服务
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceOptions, consul);
            }); 
            #endregion

            app.UseMvc();

            #region 数据库初始化
            InitDataBase(app); 
            #endregion
        }


        /// <summary>
        /// 向consul注册服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceOptions"></param>
        /// <param name="consul"></param>
        private void RegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
            .Addresses
            .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                //serviceid必须是唯一的，以便以后再次找到服务的特定实例，以便取消注册。这里使用主机和端口以及实际的服务名

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };
                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
        }
        /// <summary>
        /// 向consul注销服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceOptions"></param>
        /// <param name="consul"></param>
        private void DeRegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>().
            Addresses.Select(p => new Uri(p));
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// 数据库初始化数据方法
        /// </summary>
        /// <param name="app"></param>
        public void InitDataBase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProjectContext>();
                if (!context.Projects.Any())
                {
                    context.Projects.Add(new Project.Domain.AggregatesModel.Project
                    {
                       Id=1,
                       UserId=1

                    });
                }
                if (!context.ProjectViewers.Any())
                {
                    context.ProjectViewers.Add(new ProjectViewer
                    {
                        ProjectId=1,
                        UserName = "test"
                    });
                }
                if (!context.ProjectVisibleRules.Any())
                {
                    context.ProjectVisibleRules.Add(new ProjectVisibleRule
                    {
                        ProjectId = 1,
                        Visible=true
                    });
                }
                if (!context.ProjectContributors.Any())
                {
                    context.ProjectContributors.Add(new ProjectContributor
                    {
                        ProjectId = 1,
                        UserName="test"
                    });
                }
                if (!context.ProjectProperties.Any())
                {
                    context.ProjectProperties.Add(new ProjectProperty
                    {
                        ProjectId = 1,
                        Text="test"
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
