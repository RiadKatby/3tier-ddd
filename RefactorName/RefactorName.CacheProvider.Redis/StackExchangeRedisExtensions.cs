using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.CacheProvider.Redis
{
    public static class StackExchangeRedisExtensions
    {
        public static T Get<T>(this IDatabase cache, string key)
        {
            return Deserialize<T>(cache.StringGet(key));
        }

        public static object Get(this IDatabase cache, string key)
        {
            return Deserialize<object>(cache.StringGet(key));
        }

        public static void Set(this IDatabase cache, string key, object value, TimeSpan? expiry = null)
        {
            cache.StringSet(key, Serialize(value), expiry);
        }

        static string Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(o, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
        }

        static T Deserialize<T>(string value)
        {
            if (value == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
        }
    }
}
