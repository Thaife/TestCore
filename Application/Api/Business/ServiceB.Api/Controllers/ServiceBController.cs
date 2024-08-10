using ApplicationCore.Model;
using ApplicationCore.Utility.Startup;
using Microsoft.AspNetCore.Mvc;

namespace ServiceB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceBController : ControllerBase
    {
        [HttpGet]
        public async Task<ServiceRespon> Test()
        {
            var x = GlobalConfigUtility.Config.Appsettings;
            return new ServiceRespon("Tvthai2");
        }
    }
}
