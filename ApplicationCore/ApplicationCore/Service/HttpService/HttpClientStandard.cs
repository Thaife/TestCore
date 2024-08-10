using ApplicationCore.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplicationCore.HttpService
{
    public class HttpClientStandard : BaseHttpClient, IHttpClientStandard 
    {
        public HttpClientStandard(HttpClient httpClient):base(httpClient)
        {

        }
        //public override async Task<ServiceRespon> CallInternalService(HttpMethod method, string fullApiUrl, Dictionary<string, string> headers, object content)
        //{

        //    return new ServiceRespon();
        //}
    }
}
