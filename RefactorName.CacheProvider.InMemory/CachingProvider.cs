using RefactorName.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.CacheProvider.InMemory
{
    /// <summary>
    /// Uses the .NET built-in MemoryCache as the caching provider.
    /// </summary>
    public class CachingProvider : ICachingProvider
    {
        private static ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        private static readonly object LockObject = new object();

        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Default, int? cacheTime = null)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            var policy = new CacheItemPolicy
            {
                Priority = priority
            };
            if (cacheTime.HasValue)
            {
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime.Value);
            }

            Cache.Set(key, value, policy);
        }

        public void Clear(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            return Cache.Any(x => x.Key == key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            value = default(T);

            try
            {
                if (!Exists(key))
                    return false;

                value = (T)Cache[key];
            }
            catch (Exception)
            {
                // ignore and use default
                return false;
            }

            return true;
        }

        public int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Default)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            lock (LockObject)
            {
                int current;
                if (!TryGet(key, out current))
                {
                    current = defaultValue;
                }

                var newValue = current + incrementValue;
                Set(key, newValue, priority);
                return newValue;
            }
        }

        public void Dispose()
        {
            // no need to do anything
        }
    }
}
