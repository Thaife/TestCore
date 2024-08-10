using ApplicationCore.HttpService;
using ApplicationCore.Model;
using ApplicationCore.Utility.Startup;
using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TestController : ControllerBase
    {
        private IHttpClientStandard _httpClientStandard;
        public TestController(IHttpClientStandard httpClientStandard)
        {
            _httpClientStandard = httpClientStandard;
        }
        [HttpGet]
        public async Task<ServiceRespon> Test()
        {
            var x = GlobalConfigUtility.Config.Appsettings;
            var y = Request.Headers;
            var z = HttpContext.Response.Headers;
            var v = Response.Headers;
            //var res = await _httpClientStandard.CallInternalService(HttpMethod.Get, "http://localhost:41480/api/ServiceB", null, null);
            var res = new ServiceRespon();
            return res;
        }
    }
}
