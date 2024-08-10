using Newtonsoft.Json;
using System;
using System.Text;

namespace ApplicationCore.Utility.Common
{
    public class ConvertUtility
    {
        public static void AddCokkie()
        {

        }
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static string Serialize(object jsonObject)
        {
            return JsonConvert.SerializeObject(jsonObject);
        }
        public static byte[] GetBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
