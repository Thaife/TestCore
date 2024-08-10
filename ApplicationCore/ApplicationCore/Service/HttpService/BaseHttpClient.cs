using ApplicationCore.Model;
using ApplicationCore.Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.HttpService
{
    public class BaseHttpClient
    {
        private HttpClient _httpClient;
        public BaseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public virtual async Task<ServiceRespon> CallInternalService(HttpMethod method, string fullApiUrl, Dictionary<string, string> headers, object content)
        {
            var result = new ServiceRespon();
            var client = new HttpClient();
            result = await SendRequestAsync(client, method, fullApiUrl, headers, content);
            return result;
        }
        public virtual async Task<ServiceRespon> SendRequestAsync(HttpClient client, HttpMethod method, string fullApiUrl, Dictionary<string, string> headers, object content)
        {
            await AddHeaderRequest(client, method, headers);
            var responseMessage = new HttpResponseMessage();
            var res = new ServiceRespon();
            if (method == HttpMethod.Get)
            {
                responseMessage = await DoSendRequestGetAsync(client, fullApiUrl);
            } else if (method == HttpMethod.Post)
            {
                HttpContent contentRequest = await GetContentRequest(client, method, content);
                responseMessage = await DoSendRequestPostAsync(client, fullApiUrl, contentRequest);
            }
            string jsonContent = await responseMessage.Content.ReadAsStringAsync();
            res = ConvertUtility.Deserialize<ServiceRespon>(jsonContent);
            return res;
        }
        public virtual async Task AddHeaderRequest(HttpClient client, HttpMethod method, Dictionary<string, string> headers)
        {
            if(headers != null)
            {
                foreach (var item in headers)
                {
                    if (client.DefaultRequestHeaders.Contains(item.Key))
                    {
                        client.DefaultRequestHeaders.Remove(item.Key);
                    }
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }
        public virtual async Task<HttpContent> GetContentRequest(HttpClient client, HttpMethod method, Object content)
        {
            string stringContent = content != null ? ConvertUtility.Serialize(content) : string.Empty;
            HttpContent contentRequest = new StringContent(stringContent, Encoding.UTF8, "application/json");
            return contentRequest;
        }

        public virtual Task<HttpResponseMessage> DoSendRequestGetAsync(HttpClient client, string fullApiUrl)
        {
            return client.GetAsync(fullApiUrl);
        }
        public virtual async Task<HttpResponseMessage> DoSendRequestPostAsync(HttpClient client, string fullApiUrl, HttpContent content)
        {
            var requestMess = new HttpRequestMessage(HttpMethod.Post, fullApiUrl)
            {
                Content = content
            };
            var res = await client.SendAsync(requestMess);
            return res;
        }
    }
}
