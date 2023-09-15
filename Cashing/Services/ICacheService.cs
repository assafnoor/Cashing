namespace Cashing.Services
{
    public interface ICacheServic
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value,DateTimeOffset ExpirationTime);
        object RemoveData(string key); 
    }
}
