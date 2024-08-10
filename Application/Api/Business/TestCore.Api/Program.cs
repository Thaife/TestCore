using ApplicationBase.Startup;

namespace TestCore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BaseStartupConfig.ProgramStart(typeof(Startup), args);
            //CreateHostBuilder(args).Build().Run();
        }
    }
}
