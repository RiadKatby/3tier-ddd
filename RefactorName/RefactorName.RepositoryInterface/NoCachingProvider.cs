using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface
{
    /// <summary>
    /// No caching is done using this provider.
    /// </summary>
    public class NoCachingProvider : ICachingProvider
    {
        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Default, int? cacheTime = null)
        {
            // do nothing
        }

        public void Clear(string key)
        {
            // do nothing
        }

        public bool Exists(string key)
        {
            return false;
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            return false;
        }

        public void Dispose()
        {

        }

        public int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Default)
        {
            return 0;
        }
    }
}
