using AspNetCoreRateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.LimitRate.RateLimitProcessor
{
    public class CoreClientRateLimitProcessor : ClientRateLimitProcessor
    {
        public CoreClientRateLimitProcessor(ClientRateLimitOptions options, IClientPolicyStore store, IProcessingStrategy processingStrategy):base(options, store, processingStrategy)
        {

        }
    }
}
