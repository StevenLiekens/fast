using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fast_api.Services
{
    public class TpItemService : ITpItemService
    {
        private readonly IGw2ApiRepository _apiRepository;
        private readonly ICacheRepository _cacheRepository;
        public TpItemService(IGw2ApiRepository client, ICacheRepository cacheRepository)
        {
            _apiRepository = client;
            _cacheRepository = cacheRepository;
        }

        public async Task<List<Item>> GetItemPricesFromApi()
        {
            var ids = await _apiRepository.FetchAllItemIdsFromApi(CancellationToken.None);
            List<int> filter = new List<int>();
            List<Item> items = new List<Item>();
            List<Item> cachedItems = new List<Item>();
            List<ItemPrice> itemPrices = new List<ItemPrice>();
            foreach (var id in ids.ToArray())
            {
                if (await _cacheRepository.CheckIfCached(id))
                {
                    var cachedItem = await _cacheRepository.ReadFromCache(id);
                    filter.Add(id);
                    cachedItems.Add(cachedItem);
                }
            }
            items.AddRange(await _apiRepository.FetchAllItemsFromApi(CancellationToken.None, filter.ToArray()));
            itemPrices.AddRange(await _apiRepository.FetchAllItemPricesFromApi(CancellationToken.None, filter.ToArray()));

            foreach (var item in items)
            {
                var itemPrice = itemPrices.FirstOrDefault(x => x.ItemPriceId == item.Id);
                if (itemPrice != null)
                {
                    item.BuyData = itemPrice.BuyData;
                    item.SellData = itemPrice.SellData;
                }
            }

            items.ForEach(async x => await _cacheRepository.WriteToCache(x));
            items = items.Concat(cachedItems).ToList();

            return items;
        }

        public async Task<List<Item>> GetItemPricesFromApi(int[] ids)
        {
            var retVal = new List<Item>();
            var idList = ids.ToList();
            var cachedItems = new List<Item>();
            foreach (var id in ids.ToList())
            {
                if (await _cacheRepository.CheckIfCached(id))
                {
                    retVal.Add(await _cacheRepository.ReadFromCache(id));
                    idList.Remove(id);
                }
                //else
                //{
                //    var item = await _apiRepository.FetchItemPriceFromApi(id);
                //    await _cacheRepository.WriteToCache(item);
                //    retVal.Add(item);
                //}
            }
            retVal.AddRange(await _apiRepository.FetchItemPricesFromApi(CancellationToken.None, idList.ToArray()));
            var remainingItems = retVal.Where(x => idList.Contains(x.Id)).ToList();
            remainingItems.ForEach(async item => await _cacheRepository.WriteToCache(item));

            return retVal;
        }
    }
}
