using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace User.Identity.Infrastructure
{
    public class ResilientHttpClientFactory
    {
        private readonly ILogger<ResilientHttpClient> _logger;
        private readonly int _retryCount;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilientHttpClientFactory(ILogger<ResilientHttpClient> logger, IHttpContextAccessor httpContextAccessor, int exceptionsAllowedBeforeBreaking = 5, int retryCount = 5)
        {
            _logger = logger;
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _retryCount = retryCount;
            _httpContextAccessor = httpContextAccessor;
        }

        public ResilientHttpClient GetResilientHttpClient()
        {
            return new ResilientHttpClient(origin => CreatePolicy(origin), _logger, _httpContextAccessor);
        }

        private Policy[] CreatePolicy(string origin)
        {
            return new Policy[] {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    _retryCount,
                    retryAttempt=>TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)),
                    (exception,timespan,retrycount,context)=>{
                        var msg=$"第{retrycount}次"+
                        $"of {context.PolicyKey}"+
                        $"at {context.OperationKey},"+
                        $"due to:{exception}";
                        //_logger.LogWarning(msg);
                        _logger.LogDebug(msg);
                    }),
                Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                    _exceptionsAllowedBeforeBreaking,
                    TimeSpan.FromMinutes(1),
                    (exception,duraton)=>{
                        _logger.LogDebug("熔断器打开");
                        //_logger.LogTrace("熔断器打开");
                    },()=>{
                        _logger.LogDebug("熔断器关闭");
                        //_logger.LogTrace("熔断器关闭");
                    })
                //扩展机制:仓壁隔离;回退 todo
            };
        }
    }
}
