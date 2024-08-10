using ApplicationCore.LimitRate.RateLimitProcessor;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.LimitRate.RateLimitMiddleware
{
    public class CoreClientRateLimitMiddleware : ClientRateLimitMiddleware
    {
        public CoreClientRateLimitMiddleware(
            RequestDelegate next,
            IProcessingStrategy processingStrategy,
            IOptions<ClientRateLimitOptions> options,
            IClientPolicyStore policyStore,
            IRateLimitConfiguration config,
            ILogger<ClientRateLimitMiddleware> logger
        ) : base(next, processingStrategy, options, policyStore, config, logger)
        {

        }
    }
}
