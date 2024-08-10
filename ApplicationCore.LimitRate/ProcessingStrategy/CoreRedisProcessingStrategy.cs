using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.LimitRate.ProcessingStrategy
{
    public class CoreRedisProcessingStrategy : RedisProcessingStrategy
    {
        public CoreRedisProcessingStrategy
        (
            IConnectionMultiplexer connectionMultiplexer, IRateLimitConfiguration config, ILogger<RedisProcessingStrategy> logger
        ): base(connectionMultiplexer, config, logger)
        {

        }
    }
}
