using ApplicationCore.Utility.Startup;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Utility.Cache
{
    public class ConnectionHelper
    {
        private static readonly object objLock = new object();
        private static ConnectionMultiplexer lazyConnection;
        //private static string lazyConnection = "1";
        static ConnectionHelper()
        {
            if (lazyConnection == null)
            {
                lock (objLock)
                {
                    if (lazyConnection == null)
                    {
                        lazyConnection = ConnectionMultiplexer.Connect(GlobalConfigUtility.Config.Appsettings.RedisURL);
                    }
                }
            }
        }
        //private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection;
            }
        }
    }
}
