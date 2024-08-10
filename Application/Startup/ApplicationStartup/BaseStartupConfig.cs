using ApplicationCore.Starup.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Startup
{
    public class BaseStartupConfig
    {
        public static void ProgramStart(Type startupType, string[] args)
        {
            ApplicationStartupUtility.BuildConfigBeforeCreateApp();
            CoreStartup.Start(startupType, args);
        }
        public static void ConfigureServices(ref IServiceCollection services, IConfiguration config)
        {
            CoreStartup.ConfigureServices(ref services, config);
            CoreStartup.InitGlobalConfig(ref services, config);
        }

        public static void Configure(IApplicationBuilder app)
        {

        }
    }
}
