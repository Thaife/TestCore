using ApplicationCore.Model;

namespace ApplicationCore.Utility.Startup
{
    public class GlobalConfigUtility
    {
        private static GlobalConfig _globalConfig = null;
        public static GlobalConfig Config
        {
            get { return _globalConfig; }
        }
        public static void InitConfig(GlobalConfig config)
        {
            if (config != null)
            {
                _globalConfig = config;
            }
        }
    }
}
