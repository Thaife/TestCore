using ApplicationCore.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplicationCore.HttpService
{
    public interface IHttpClientStandard
    {
        Task<ServiceRespon> CallInternalService(HttpMethod method, string fullApiUrl, Dictionary<string, string>headers, object content);
    }
}
