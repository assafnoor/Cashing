
using StackExchange.Redis;
using System.Text.Json;

namespace Cashing.Services
{
    public class RedisService : ICacheServic
    {
        private IDatabase _database;
        public RedisService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _database = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
           var result=_database.StringGet(key);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<T>(result);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var result=_database.KeyExists(key);
            if(result)
                return _database.KeyDelete(key);
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset ExpirationTime)
        {
            var expirytime = ExpirationTime.DateTime.Subtract(DateTime.Now);
            return  _database.StringSet(key,JsonSerializer.Serialize(value),expirytime);
           
        }
    }
}
