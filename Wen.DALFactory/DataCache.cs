using SqlSugar;
using System.Runtime.Caching;
using Wen.Common;
namespace Wen.DALFactory 
{
/// <summary>
/// 缓存操作类
/// </summary>
public class DataCache : ICacheService
{
	private static readonly int seconds = PubConstant.GetModelCache;

	public static ObjectCache cache = MemoryCache.Default;//声明缓存类

       

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
	{
		return cache[CacheKey];
	}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <param name="objObject"></param>
		public static void SetCache(string CacheKey, object objObject)
		{
		

		if (objObject == null)
			return;
		ObjectCache cache = MemoryCache.Default;//声明缓存类
		CacheItemPolicy policy = new CacheItemPolicy()
		{
			AbsoluteExpiration = DateTime.Now.AddSeconds(seconds)
		};
		cache.Set(CacheKey, objObject, policy);
	    }

	public static void Remove(string key)
	{
		MemoryCache.Default.Remove(key);
	}

    public void Add<V>(string key, V value)
    {
        ObjectCache cache = MemoryCache.Default;
        cache[key] = value;
    }
    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        MemoryCache.Default.Add(key, value, new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(cacheDurationInSeconds)
        });
    }
    public bool ContainsKey<V>(string key)
    {
        return MemoryCache.Default.Contains(key);
    }
    public V Get<V>(string key)
    {
        return (V)MemoryCache.Default.Get(key);
    }
    public IEnumerable<string> GetAllKey<V>()
    {
        var keys = new List<string>();
        foreach (var cacheItem in MemoryCache.Default)
        {
            keys.Add(cacheItem.Key);
        }
        return keys;
    }
    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
    {
        var cacheManager = MemoryCache.Default;
        if (cacheManager.Contains(cacheKey))
        {
            return (V)cacheManager[cacheKey];
        }
        else
        {
            var result = create();
            cacheManager.Add(cacheKey, result, new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(cacheDurationInSeconds)
            });
            return result;
        }
    }
    public void Remove<V>(string key)
    {
        MemoryCache.Default.Remove(key);
    }
  }
}