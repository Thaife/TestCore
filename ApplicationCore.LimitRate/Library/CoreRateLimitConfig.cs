using ApplicationCore.LimitRate.RateLimitMiddleware;
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ApplicationCore.LimitRate.Library
{
    public static class CoreRateLimitConfig
    {
        /// <summary>
        /// Hàm cấu hình limit request
        /// </summary>
        public static void ConfigureRateLimit(this IServiceCollection services, IConfiguration config)
        {
            ConfigRateLimitSettings(services, config);
            ConfigRateLimitCounterStore(services, config);
            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, CoreRateLimitConfiguration>();
        }
        /// <summary>
        /// Hàm cấu hình store lưu thông tin limit request
        /// </summary>
        public static void ConfigRateLimitCounterStore(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect("127.0.0.1:6379"));
            services.AddDistributedRateLimiting<RedisProcessingStrategy>();
            //
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    EndPoints = { "127.0.0.1:6379" },
                    //silently retry in the background if the Redis connection is temporarily down
                    AbortOnConnectFail = false
                };
                options.Configuration = "127.0.0.1:6379";
                options.InstanceName = "AspNetRateLimit";
            });
            //
            //2 thằng này sử dụng store là IDistributeCache (services.AddStackExchangeRedisCache)
            // inject counter and rules distributed cache stores
            services.AddSingleton<IClientPolicyStore, DistributedCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
        }
        public static void ConfigRateLimitSettings(IServiceCollection services, IConfiguration config)
        {
            //load general configuration from appsettings.json
            services.Configure<ClientRateLimitOptions>(config.GetSection("ClientRateLimiting"));

            //load client rules from appsettings.json
            services.Configure<ClientRateLimitPolicies>(config.GetSection("ClientRateLimitPolicies"));
        }
        public static void UseRateLimit(this WebApplication app)
        {
            app.UseMiddleware<CoreClientRateLimitMiddleware>();
        }
    }
}
