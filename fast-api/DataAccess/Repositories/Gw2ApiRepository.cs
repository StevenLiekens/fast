using fast_api.Config;
using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using fast_api.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoreLinq;

namespace fast_api.DataAccess.Repositories
{
    public class Gw2ApiRepository : IGw2ApiRepository
    {
        private readonly Gw2ApiClient _gw2ApiClient;

        public Gw2ApiRepository(Gw2ApiClient gw2ApiClient)
        {
            _gw2ApiClient = gw2ApiClient;
        }

        public async Task<List<int>> FetchAllItemIdsFromApi(CancellationToken cancellationToken)
        {
            return await _gw2ApiClient.FetchAllItemIdsAsync(cancellationToken);
        }

        public async Task<List<Item>> FetchAllItemsFromApi(CancellationToken cancellationToken, int[] filter)
        {
            var ids = await _gw2ApiClient.FetchAllItemIdsAsync(cancellationToken);
            var chunkedItemIds = ids.Where(x => !filter.Contains(x)).Batch(150);
            var itemTasks = chunkedItemIds.Select(chunk => _gw2ApiClient.FetchItemsForIdsAsync(cancellationToken, chunk))
                .ToList();
            var resolvedItems = (await Task.WhenAll(itemTasks)).SelectMany(x => x).ToList();
            return resolvedItems;
        }

        public async Task<List<ItemPrice>> FetchAllItemPricesFromApi(CancellationToken cancellationToken, int[] filter)
        {
            var ids = await _gw2ApiClient.FetchAllItemIdsAsync(cancellationToken);
            var chunkedItemIds = ids.Where(x => !filter.Contains(x)).Batch(150);
            var tasks = chunkedItemIds
                .Select(chunk => _gw2ApiClient.FetchItemPricesForIdsAsync(cancellationToken, chunk))
                .ToList();

            return (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();
        }

        public async Task<List<Item>> FetchItemPricesFromApi(CancellationToken cancellationToken, int[] itemIds)
        {
            if (itemIds == null || itemIds.Length == 0)
            {
                return new List<Item>();
            }

            var items = await _gw2ApiClient.FetchItemsForIdsAsync(cancellationToken, itemIds);
            var itemPrices = await _gw2ApiClient.FetchItemPricesForIdsAsync(cancellationToken, itemIds);

            items.ForEach(x =>
            {
                x.BuyData = itemPrices.FirstOrDefault(y => y.ItemPriceId == x.Id)?.BuyData;
                x.SellData = itemPrices.FirstOrDefault(y => y.ItemPriceId == x.Id)?.SellData;
            });

            return items;
        }
    }
}