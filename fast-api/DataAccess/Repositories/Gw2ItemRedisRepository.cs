using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using Serilog;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace fast_api.DataAccess.Repositories
{
    public class Gw2ItemRedisRepository : ICacheRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public Gw2ItemRedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer ?? null;
        }
        public async Task<bool> CheckIfCached(int id)
        {
            var db = _connectionMultiplexer?.GetDatabase();
            if(db == null)
            {
                Log.Error("Error accessing redis db.");
                return false;
            }
            var item = await db.StringGetAsync(id.ToString());

            return !item.IsNullOrEmpty;
        }

        public async Task WriteToCache(Item item)
        {
            var db = _connectionMultiplexer?.GetDatabase();
            if (db != null)
            {
                var json = JsonSerializer.Serialize(item, new JsonSerializerOptions() { MaxDepth = 3 });
                await db.StringSetAsync(item.Id.ToString(), json, new TimeSpan(0, 10, 0));
            }
            else
            {
                Log.Error("Error accessing redis db.");
            }
        }

        public async Task<Item> ReadFromCache(int id)
        {
            var db = _connectionMultiplexer?.GetDatabase();
            if (db == null)
            {
                Log.Error("Error accessing redis db.");
                return null;
            }
            else
            {
                var json = await db.StringGetAsync(id.ToString());
                return JsonSerializer.Deserialize<Item>(json);
            }
        }
    }
}
