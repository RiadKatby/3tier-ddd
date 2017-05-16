using RefactorName.RepositoryInterface;
using StackExchange.Redis;
using System;
using System.Runtime.Caching;

namespace RefactorName.CacheProvider.Redis
{
    public class CachingProvider : ICachingProvider
    {
        protected IDatabase Redis { get; set; }

        public CachingProvider() : this("localhost")
        {
        }

        public CachingProvider(string host, bool ssl = true)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentNullException("host");

            var configOptions = new ConfigurationOptions
            {
                EndPoints = { {host} },
                Ssl = ssl
            };

            Initialize(configOptions);
        }

        public CachingProvider(string host, int port, bool ssl = true)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentNullException("host");

            var configOptions = new ConfigurationOptions
            {
                EndPoints = { {host, port} },
                Ssl = ssl
            };

            Initialize(configOptions);
        }

        public CachingProvider(string host, int port, string password, bool ssl = true)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host");

            var configOptions = new ConfigurationOptions
            {
                EndPoints = { { host, port} },
                Ssl = ssl,
                Password = password
            };

            Initialize(configOptions);
        }

        public CachingProvider(ConfigurationOptions configOptions)
        {
            if (configOptions == null)
                throw new ArgumentNullException("configOptions");

            Initialize(configOptions);
        }

        private void Initialize(ConfigurationOptions configOptions)
        {
            if (RedisConnector.Connection == null)
            {
                configOptions.AbortOnConnectFail = false;
                RedisConnector.Connection = ConnectionMultiplexer.Connect(configOptions);
            }

            Redis = RedisConnector.Connection.GetDatabase();
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Clear(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            Redis.KeyDelete(key);
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            return Redis.KeyExists(key);
        }

        public int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = 0)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            // no need to use a lock since the redis increment method is atomic already
            return Convert.ToInt32(Redis.StringIncrement(key, defaultValue));
        }

        /// <summary>
        /// Insert value into the cache using appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="value">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="priority"></param>
        /// <param name="timeoutInSeconds">Seconds to cache</param>
        public void Set<T>(string key, T value, CacheItemPriority priority = 0, int? timeoutInSeconds = default(int?))
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            TimeSpan? expiry = null;
            if (timeoutInSeconds.HasValue)
            {
                expiry = new TimeSpan(0, 0, 0, timeoutInSeconds.Value);
            }

            Redis.Set(key, value, expiry);
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if
        /// item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        public bool TryGet<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            try
            {
                value = Redis.Get<T>(key);

                if (Equals(value, default(T)))
                {
                    value = default(T);
                    return false;
                }
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }
    }
}
