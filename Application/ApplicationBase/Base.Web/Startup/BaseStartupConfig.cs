using ApplicationCore.Web.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Web.Startup
{
    public class BaseStartupConfig
    {
        public static void ProgramStart(IConfigurationBuilder configuration)
        {
            CoreStartup.ProgramStart(configuration);
        }
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            CoreStartup.ConfigureServices(services, config);
        }

        public static void ConfigureApp(WebApplication app)
        {
            CoreStartup.ConfigureApp(app);
        }
    }
}
