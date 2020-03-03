using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience.Consul;
using Resilience.Http;
using User.Identity.Autentication;
using User.Identity.Infrastructure;
using User.Identity.Services;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace User.Identity
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5001";
                    options.Audience = "user_api";
                    options.RequireHttpsMetadata = false;
                });
            services.AddIdentityServer()
                //自定义验证
                .AddExtensionGrantValidator<SmsAuthCodeValidator>()
               .AddDeveloperSigningCredential()
               //测试使用内存
               .AddInMemoryClients(Config.GetClients())
               .AddInMemoryApiResources(Config.GetResource())
               .AddInMemoryIdentityResources(Config.GetIdentityResource());
            services.AddScoped<IAuthCodeService, TestAuthCodeService>()
                .AddScoped<IUserService, UserService>();

            #region ProfileService
            services.AddTransient<IProfileService, ProfileService>(); 
            #endregion

            #region 向Consul注册api服务
            //注册服务发现
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            }); 
            #endregion

            #region 未使用DnsQuery集成依赖注入(使用httpclient测试)
            //未使用DnsQuery依赖注入
            //services.AddSingleton(new HttpClient()); 
            #endregion

            #region 集成Polly处理服务之间调用故障使用ResilientHttpClientFactory,ResilientHttpClient
            //注册ResilientHttpClientFactory全局单例
            services.AddSingleton(typeof(ResilientHttpClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                int retryCount = 4;
                int exceptionsAllowedBeforeBreaking = 4;
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseIdentityServer();
            //在管道中注册zipkin
            RegisterZipkinService(app, loggerFactory, applicationLifetime);
            app.UseMvc();
        }


        /// <summary>
        /// 注册Zipkin
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="applicationLifetime"></param>
        private void RegisterZipkinService(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStarted.Register(() => {
                TraceManager.SamplingRate = 1.0f;//记录数据密度，1.0代表全部记录
                var logger = new TracingLogger(loggerFactory, "zipkin4net");//内存数据
                var httpSender = new HttpZipkinSender("http://localhost:9411", "application/json");

                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());//注册zipkin
                var consoleTracer = new zipkin4net.Tracers.ConsoleTracer();//控制台输出

                TraceManager.RegisterTracer(tracer);//注册
                TraceManager.RegisterTracer(consoleTracer);//控制台输入日志
                TraceManager.Start(logger);//放到内存中的数据
            });
            applicationLifetime.ApplicationStopped.Register(() => TraceManager.Stop());

            app.UseTracing("identityapi");//这边的名字可自定义
        }
    }
}
