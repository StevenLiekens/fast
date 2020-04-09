using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using StackExchange.Redis;
using System;
using System.Text.Json;
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
            var json = JsonSerializer.Serialize(item, new JsonSerializerOptions() { MaxDepth = 3 });
            await db.StringSetAsync(item.Id.ToString(), json, new TimeSpan(0, 10, 0));
        }

        public async Task<Item> ReadFromCache(int id)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = await db.StringGetAsync(id.ToString());
            var test = JsonSerializer.Deserialize<Item>(json);
            return JsonSerializer.Deserialize<Item>(json);
        }
    }
}
