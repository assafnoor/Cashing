using System.Runtime.Caching;

namespace Cashing.Services
{
    public class CacheService : ICacheServic
    {
        private ObjectCache _memorcach = MemoryCache.Default;
        public T GetData<T>(string key)
        {
            try
            {
                T item = (T) _memorcach.Get(key);
                return item;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object RemoveData(string key)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                  var result =  _memorcach.Remove(key);
                }
                else
                {
                        res = false;
                }
                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
         
        }

        public bool SetData<T>(string key, T value, DateTimeOffset ExpirationTime)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                    _memorcach.Set(key, value, ExpirationTime);
                else
                    res = false;
                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
