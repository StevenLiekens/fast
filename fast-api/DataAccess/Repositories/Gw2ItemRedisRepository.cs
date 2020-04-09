using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace fast_api.DataAccess.Repositories
{
    public class Gw2ItemRedisRepository : ICacheRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public Gw2ItemRedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task<bool> CheckIfCached(int id)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var item = await db.StringGetAsync(id.ToString());

            return !item.IsNullOrEmpty;
        }

        public async Task WriteToCache(Item item)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = JsonConvert.SerializeObject(item);
            await db.StringSetAsync(item.Id.ToString(), json, new TimeSpan(0, 10, 0));
        }

        public async Task<Item> ReadFromCache(int id)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = await db.StringGetAsync(id.ToString());
            return JsonConvert.DeserializeObject<Item>(json);
        }
    }
}
