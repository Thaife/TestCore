using ApplicationCore.Interface.Cache;
using ApplicationCore.Model;
using ApplicationCore.Utility.Common;
using ApplicationCore.Utility.HttpClient;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Service.MiddleWare
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private TimeSpan _timeWindow;
        private long _limitCount;
        private ICacheService _cacheService;

        public RequestMiddleware(RequestDelegate next, ICacheService cacheService, TimeSpan timeWindow, long limitCount)
        {
            _next = next;
            _timeWindow = timeWindow;
            _limitCount = limitCount;
            _cacheService = cacheService;
        }
        public string ConvertDateTimeToStringWithCharactor(DateTime dateConvert, string dateFormat, string charactor)
        {
            if(string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = "dd-MM-yyyy";
            }
            //string dateString = dateConvert.ToString(dateFormat);
            string day = dateConvert.Day.ToString();
            string month = dateConvert.Month.ToString();
            string year = dateConvert.Year.ToString();
            string hour = dateConvert.Hour.ToString();
            string minute = dateConvert.Minute.ToString();
            string second = dateConvert.Second.ToString();
            var res = $"{day}{charactor}{month}{charactor}{year}{charactor}{hour}{charactor}{minute}{charactor}{second}";
            return res;
        }
        /// <summary>
        /// Hàm này chưa cho vào utility được, cần phải sửa để linh hoạt hơn
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="dateFormat"></param>
        /// <param name="charactor"></param>
        /// <returns></returns>
        public DateTime ConvertStringWithCharactorToDateTime(string dateString, string dateFormat, string charactor)
        {
            var arr = dateString.Split(charactor);
            string day = "", month = "", year = "", hour = "", min = "", sec = "";
            if(string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = "dd-MM-yyyy-H-m-s";
            }
            switch (dateFormat)
            {
                case "dd-MM-yyyy-H-m-s":
                case "dd/MM/yyyy/H/m/s":
                    day = arr[0];
                    month = arr[1];
                    year = arr[2];
                    hour = arr[3];
                    min = arr[4];
                    sec = arr[5];
                    break;
                default:
                    break;
            }
            if(string.IsNullOrEmpty(day))
            {
                day = DateTime.Now.Date.ToString();
                month = DateTime.Now.Month.ToString();
                year = DateTime.Now.Year.ToString();
            }
            string dateTimeString = $"{year}-{month}-{day}-{hour}-{min}-{sec}";
            string[] formats = { "yyyy-M-d-H-m-s", "yyyy-MM-dd-H-m-ss" };
            DateTime.TryParseExact(dateTimeString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res);
            return res;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //Pass validdate middleware hay không.
            bool isPass = true;
            //IP client
            string ipClient = HttpClientUtility.GetClientIp(context);
            //Biến check trạng thái commit của transaction +1 limit count
            bool isCommit = false;
            int count = 0;
            while (!isCommit && count <= 15)
            {
                var limitRequestString = _cacheService.GetData<string>(ipClient);
                //Nếu request này đã có phiên
                if (!string.IsNullOrEmpty(limitRequestString))
                {
                    var limitRequest = ConvertUtility.Deserialize<Dictionary<string, long>>(limitRequestString);
                    string dateStartSessionStr = limitRequest.Keys.First();
                    //DateTime dateStartSession = Convert.ToDateTime(dateStartSessionStr.Split("utc_")[1]);
                    DateTime dateStartSession = ConvertStringWithCharactorToDateTime(dateStartSessionStr.Split("time_")[1], null, "_");
                    long countSession = limitRequest.Values.First();
                    //Nếu vẫn còn trong phiên thì check tiếp
                    if (dateStartSession + _timeWindow > DateTime.Now)
                    {
                        //Nếu lượt request trong phiên này đã lớn hơn giới hạn thì set pass = false để không thể pass dc validate này sau khi out vòng lặp while
                        if (countSession + 1 > _limitCount)
                        {
                            isPass = false;
                        }
                        var newLimit = new Dictionary<string, long> { { dateStartSessionStr, countSession + 1 } };
                        var newLimitJson = ConvertUtility.Serialize(newLimit);
                        var trans = _cacheService.CreateTransaction();
                        //trans.AddCondition(Condition.StringEqual(ipClient, limitRequestString));
                        trans.StringSetAsync(ipClient, JsonConvert.SerializeObject(newLimitJson), _timeWindow - (DateTime.Now- dateStartSession));
                        var setSuccess = await trans.ExecuteAsync();
                        if (setSuccess)
                        {
                            isCommit = true;
                            break;
                        }
                    } else
                    {
                        _cacheService.RemoveData(ipClient);
                        var newDate = ConvertDateTimeToStringWithCharactor(DateTime.Now, "", "_");
                        if(!string.IsNullOrEmpty(newDate))
                        {
                            newDate = $"time_{newDate}";
                            var newLimitJson = ConvertUtility.Serialize(new Dictionary<string, long> { { newDate, 1 } });
                            _cacheService.SetData<string>(ipClient, newLimitJson, _timeWindow);
                            isCommit = true;
                            break;
                        }
                    }
                } else
                {
                    var newDate = ConvertDateTimeToStringWithCharactor(DateTime.Now, "", "_");
                    if (!string.IsNullOrEmpty(newDate))
                    {
                        newDate = $"time_{newDate}";
                        var newLimitJson = ConvertUtility.Serialize(new Dictionary<string, long> { { newDate, 1 } });
                        _cacheService.SetData<string>(ipClient, newLimitJson, _timeWindow);
                        isCommit = true;
                        break;
                    }
                }
                count++;
            }
            //UserInfor user = new UserInfor();
            //user.IpAddress = ipClient;
            //user.FullName = "Trần Văn Thái";
            //context.Items.Add("Context", user);
            //Nếu chưa đạt đến giới hạn limit và phải đảm bảo isCommit=true(tức vòng lặp while phải được break chủ động) thì mới cho pass
            if(isPass && isCommit)
            {
                await _next(context);
            }else
            {
                context.Response.StatusCode = 429;
                context.Response.ContentType = "text/plain";
                context.Response.WriteAsync("Giới hạn request!");
                return;
            }
        }
    }
}