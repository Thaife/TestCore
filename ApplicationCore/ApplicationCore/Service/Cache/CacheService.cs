using ApplicationCore.Interface.Cache;
using ApplicationCore.Utility.Cache;
using ApplicationCore.Utility.Startup;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace ApplicationCore.Service.Cache
{
    public class CacheService : ICacheService
    {
        private IDatabase _db;
        public CacheService()
        {
            ConfigureRedis();
        }
        private void ConfigureRedis()
        {
            _db = ConnectionHelper.Connection.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public bool SetData<T>(string key, T value, TimeSpan? expiration = null)
        {
            TimeSpan expirationTime = TimeSpan.FromSeconds(0);
            if(expiration != null)
            {
                expirationTime = expiration.Value;
            }
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expirationTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        public ITransaction CreateTransaction(object? asyncState = null)
        {
            return _db.CreateTransaction(asyncState);
        }
    }
}
