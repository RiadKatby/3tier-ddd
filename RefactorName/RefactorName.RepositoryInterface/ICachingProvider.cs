using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface
{
    /// <summary>
    /// Represent main functionality of any Caching Provider.
    /// </summary>
    public interface ICachingProvider : IDisposable
    {
        /// <summary>
        /// Set specified value in cache.
        /// </summary>
        /// <typeparam name="T">Type of value that will be cached.</typeparam>
        /// <param name="key">key of cached item.</param>
        /// <param name="value">value that will be cached.</param>
        /// <param name="priority">the proiorty of cached value.</param>
        /// <param name="timeoutInSeconds">life length of cache item, null if it is forever.</param>
        void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Default, int? timeoutInSeconds = null);

        /// <summary>
        /// Remove specific cache item.
        /// </summary>
        /// <param name="key">key of cache item that will be removed.</param>
        void Clear(string key);

        /// <summary>
        /// Determmines if specific key already cached or not.
        /// </summary>
        /// <param name="key">key of cached item.</param>
        /// <returns>True, if item already cached. False otherwise.</returns>
        bool Exists(string key);

        /// <summary>
        /// Trying to get cached item from cache.
        /// </summary>
        /// <typeparam name="T">Type of item to get.</typeparam>
        /// <param name="key">key of cached item.</param>
        /// <param name="value">the value if it is already cached. Default value if it is not cached.</param>
        /// <returns>True, if item exist in cache. False otherwise.</returns>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// Retrieve the specified value of <paramref name="key"/> and increment it by <paramref name="incrementValue"/>, if value is not already stored <paramref name="defaultValue"/> will be used.
        /// </summary>
        /// <param name="key">key of cached item.</param>
        /// <param name="defaultValue">default value that will be incremented if no already stored value.</param>
        /// <param name="incrementValue">the value that will increase the value of key.</param>
        /// <param name="priority">cache item priority.</param>
        /// <returns></returns>
        int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Default);

    }
}
