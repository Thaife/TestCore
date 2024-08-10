using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;

namespace ApplicationCore.Utility.HttpClient
{
    public static class HttpClientUtility
    {
        public static Dictionary<string, (DateTime, int)> _limitRequest = new Dictionary<string, (DateTime, int)>();
        public static string GetClientIp(HttpContext context)
        {
            var localIp = context.Connection.LocalIpAddress;
            var remoteIp = context.Connection.RemoteIpAddress;
            IPAddress ipv4 = remoteIp.MapToIPv4();
            return ipv4.ToString();


        }
    }
}
