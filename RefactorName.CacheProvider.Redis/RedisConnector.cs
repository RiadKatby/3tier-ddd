using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.CacheProvider.Redis
{
    public static class RedisConnector
    {
        public static ConnectionMultiplexer Connection { get; set; }
    }
}
