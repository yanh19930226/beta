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
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recommend.Api.Data;
using Recommend.Api.Infrastructure;
using Recommend.Api.IntergrationHandler;
using Recommend.Api.Models;
using Recommend.Api.Services;
using Resilience.Consul;
using Resilience.Http;
using Swashbuckle.AspNetCore.Swagger;

namespace Recommend.Api
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

            services.AddScoped<IUserService, UserService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<ProjectCreateIntergrationEventHandler>();

            #region MySql
            services.AddDbContext<RecommendContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            });
            #endregion

            #region 认证授权
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5001";
                    options.Audience = "recommend_api";
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
                options.SwaggerDoc("Recommend.Api", new Info { Title = "Recommend.Api", Version = "v1" });
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
                    opt.NodeId = "4";
                    opt.NodeName = "CAP No.4 Node";
                });
            });
            #endregion

            #region 集成Polly处理服务之间调用故障使用ResilientHttpClientFactory,ResilientHttpClient
            //注册ResilientHttpClientFactory全局单例
            services.AddSingleton(typeof(ResilientHttpClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                //var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                int retryCount = 4;
                int exceptionsAllowedBeforeBreaking = 4;
                var httpContextAccessor = new HttpContextAccessor();
                return new ResilientHttpClientFactory(logger, httpContextAccessor, retryCount, exceptionsAllowedBeforeBreaking);
            });
            //注册ResilientHttpClient全局单例
            services.AddSingleton<IHttpClient>(sp =>
            {
                return sp.GetRequiredService<ResilientHttpClientFactory>().GetResilientHttpClient();
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
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/Recommend.Api/swagger.json", "Recommend.Api"); });
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

            InitDataBase(app);
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
                var context = scope.ServiceProvider.GetRequiredService<RecommendContext>();
                if (!context.ProjectRecommends.Any())
                {
                    context.ProjectRecommends.Add(new ProjectRecommend
                    {
                        Id = 1,
                        UserId = 1,
                        FromUserId=8,
                        FromUserName="",
                        FromUserAvatar="test",
                        ProjectId=1,
                        CreateTime=DateTime.Now
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
