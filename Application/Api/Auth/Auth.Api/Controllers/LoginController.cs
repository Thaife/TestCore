using ApplicationCore.Interface.Cache;
using ApplicationCore.Library.AuthLibraryCore;
using ApplicationCore.Model;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ICacheService _cacheService;
        public LoginController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        /// <summary>
        /// Test login 1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("1")]
        public ServiceRespon Login(Dictionary<string, string> data)
        {
            var res = new ServiceRespon();
            var testSet = _cacheService.SetData("key1", 1, TimeSpan.FromHours(1));
            var testGet = _cacheService.GetData<object>("key1");
            string account = data["account"];
            string password = data["password"];
            string securityKeyString = "AnhThaiDepTrai17092001@misatraluongthapvcd";
            //Lấy chuỗi băm so sánh với chuỗi băm trong db, khớp thì tạo tiếp token phiên đăng nhập
            var strRncodeSHA256 = AuthLibraryCore.GetBitStringEncryptHASH256WithKey($"{account}{password}", securityKeyString);
            string jwtString = AuthLibraryCore.CreateJWTAfterLogin(account);
            return res.OnSuccess(jwtString);
        }
        /// <summary>
        /// Test login 2
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("2")]
        public ServiceRespon Login2(Dictionary<string, string> data)
        {
            var res = new ServiceRespon();

            string account = data["account"];
            string password = data["password"];
            string securityKeyString = "AnhThaiDepTrai17092001@misatraluongthapvcd";
            //Lấy chuỗi băm so sánh với chuỗi băm trong db, khớp thì tạo tiếp token phiên đăng nhập
            var strRncodeSHA256 = AuthLibraryCore.GetBitStringEncryptHASH256WithKey($"{account}{password}", securityKeyString);
            string jwtString = AuthLibraryCore.CreateJWTAfterLogin(account);
            return res.OnSuccess(jwtString);
        }
    }
}
