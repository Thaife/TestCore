using ApplicationCore.Utility.Startup;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Service.MiddleWare
{
    public class AuthContextMiddleWare
    {
        private readonly RequestDelegate _next;

        public AuthContextMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //Set header
            SetHeader(context);
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
        private void SetHeader(HttpContext context)
        {
            //Set header request
            var token = context.Request.Headers["Authorization"];

            //Set header Respon
            var requestConfig = GlobalConfigUtility.Config.RequestConfig;
            if(requestConfig != null && requestConfig.Count > 0)
            {
                foreach (var item in requestConfig)
                {
                    string key = item.Key;
                    if (context.Response.Headers.ContainsKey(key))
                    {
                        context.Response.Headers.Remove(key);
                    }
                    context.Response.Headers.Add(key, item.Value);
                }
            }
        }
    }
}
