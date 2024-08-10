using ConfigServiceTest;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigServiceTest
{
    public static class ConfigSomethingFactoryTest
    {
        public static void ConfiguraionSomethingService(this IServiceCollection services)
        {
            services.AddSingleton<ServiceTest>();
            //services.AddSingleton....

        }
    }
}
