using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        public IDatabase db { get; set; }
        private ConnectionMultiplexer _redis;
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }
        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);

        }
        /// <summary>
        /// redis server üzerinden bulunan dblerden birini alıyoruz. Redis desktop manager da 16 tane db görünüyor.
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
