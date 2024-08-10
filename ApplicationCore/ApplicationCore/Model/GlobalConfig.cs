using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model
{
    public class GlobalConfig
    {
        public Appsettings Appsettings { get; set; }
        public Dictionary<string,string> RequestConfig { get; set; }
    }
    public class Appsettings
    {
        public string AppCode { get; set; }
        public string JwtSecretKey { get; set; }
        public string RedisURL { get; set; }
    }
    
}
