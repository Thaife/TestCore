using ApplicationCore.Utility;
using ConfigServiceTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace TestCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCoreController : ControllerBase
    {
        public TestCoreController(IConfiguration configuration)
        {
            var x = configuration;
        }
        [HttpGet("v1")]
        public async Task<IActionResult> Test()
        {
            return Ok("TVTHAI");
        }
    }
}
