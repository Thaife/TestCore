using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.LimitRate.Library
{
    public class CoreRateLimitConfiguration : RateLimitConfiguration
    {
        public CoreRateLimitConfiguration(
            IOptions<IpRateLimitOptions> ipOptions,
            IOptions<ClientRateLimitOptions> clientOptions
        ):base(ipOptions, clientOptions)
        {

        }
        /// <summary>
        /// Chưa rõ mục đích, tạm thời đang copy ở hướng dẫn git library
        /// </summary>
        public override void RegisterResolvers()
        {
            base.RegisterResolvers();

            //ClientResolvers.Add(new ClientQueryStringResolveContributor(HttpContextAccessor, ClientRateLimitOptions.ClientIdHeader));
        }
    }
}
