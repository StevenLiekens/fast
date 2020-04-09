using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fast_api.Services
{
    public class TpItemService : ITpItemService
    {
        private readonly IApiFetchClient _client;
        private readonly ICacheRepository _cacheRepository;
        public TpItemService(IApiFetchClient client, ICacheRepository cacheRepository)
        {
            _client = client;
            _cacheRepository = cacheRepository;
        }

        public async Task<List<Item>> GetItemPricesFromApi(int[] ids)
        {
            var retVal = new List<Item>();
            var idList = ids.ToList();
            try
            {
                foreach (var id in idList)
                {
                    if (await _cacheRepository.CheckIfCached(id))
                    {
                        retVal.Add(await _cacheRepository.ReadFromCache(id));
                        idList.Remove(id);
                    }
                    //else
                    //{
                    //    var item = await _client.FetchItemPriceFromApi(id);
                    //    await _cacheRepository.WriteToCache(item);
                    //    retVal.Add(item);
                    //}
                }
                retVal.AddRange(await _client.FetchItemPricesFromApi(idList.ToArray()));
                var remainingItems = retVal.Where(x => !idList.Contains(x.Id)).ToList();
                remainingItems.ForEach(async item => await _cacheRepository.WriteToCache(item));
            }
            catch(RedisConnectionException ex)
            {
                return await _client.FetchItemPricesFromApi(idList.ToArray());
            }

            return retVal;
        }
    }
}
