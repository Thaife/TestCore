using ApplicationCore.Starup.Service;

namespace Application.Startup
{
    internal class ApplicationStartupUtility
    {
        internal static void BuildConfigBeforeCreateApp()
        {
            CoreStartup.BuildConfigBeforeCreateApp();
            //Có thể build custom config ở dưới này
        }
    }
}
