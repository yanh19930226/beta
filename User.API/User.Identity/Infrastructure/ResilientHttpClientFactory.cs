using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Obsolete]
        public ResilientHttpClient GetResilientHttpClient()
        {
            return new ResilientHttpClient(origin => CreatePolicy(origin), _logger, _httpContextAccessor);
        }

        [Obsolete]
        private Policy[] CreatePolicy(string origin)
        {
            return new Policy[] {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetry(
                    _retryCount,
                    retryAttempt=>TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)),
                    (exception,timespan,retrycount,context)=>{
                        var msg=$"第{retrycount}"+
                        $"of {context.PolicyKey}"+
                        $"at {context.ExecutionKey},"+
                        $"due to:{exception}";
                        _logger.LogWarning(msg);
                        _logger.LogDebug(msg);
                    }),
                Policy.Handle<HttpRequestException>()
                .CircuitBreaker(
                    _exceptionsAllowedBeforeBreaking,
                    TimeSpan.FromMinutes(1),
                    (exception,duraton)=>{
                        _logger.LogTrace("熔断器打开");
                    },()=>{
                        _logger.LogTrace("熔断器关闭");
                    })
            };
        }
    }
}
