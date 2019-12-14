﻿using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resilience.Consul;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using User.API.Data;
using User.API.Filters;

namespace User.API
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
            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MysqlUser"));
                //options.UseInMemoryDatabase()
            });
            //认证授权
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5001";
                    options.Audience = "user_api";
                    options.RequireHttpsMetadata = false;
                });
            //Swagger配置
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("User.Api", new Info { Title = "User.Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            //读取consul相关配置
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GloabalExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //CAP
            services.AddCap(options =>
            {
                options.UseEntityFramework<UserContext>()
                .UseMySql("server=localhost;port=3306;database=beta_contact;userid=yanh;password=123")
                .UseRabbitMQ("localhost")
                .UseDashboard();

                options.UseDiscovery(opt =>
                {
                    opt.DiscoveryServerHostName = "localhost";
                    opt.DiscoveryServerPort = 8500;
                    opt.CurrentNodeHostName = "localhost";
                    opt.CurrentNodePort = 5800;
                    opt.NodeId = "1";
                    opt.NodeName = "CAP No.1 Node";
                });

            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserContext userContext, IApplicationLifetime applicationLifetime,
            IOptions<ServiceDisvoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            userContext.Database.Migrate();
            if (!userContext.Users.Any())
            {
                userContext.Users.Add(new Models.AppUser
                {
                    Name = "yanh"
                });
                userContext.SaveChanges();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //认证授权
            app.UseAuthentication();
            //Swagger配置
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/User.Api/swagger.json", "User.Api"); });
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
            app.UseMvc();
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
        /// 数据库初始化已弃用
        /// </summary>
        /// <param name="app"></param>
        public void InitUserDataBase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
                if (!userContext.Users.Any())
                {
                    userContext.Users.Add(new Models.AppUser
                    {
                        Name = "yanh"
                    });
                    userContext.SaveChanges();
                }
            }
        }
    }
}
