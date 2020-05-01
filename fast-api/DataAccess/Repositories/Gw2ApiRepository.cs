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
        private readonly Gw2ApiEndpoints _gw2ApiEndpoints;
        private readonly Gw2ApiClient _gw2ApiClient;

        public Gw2ApiRepository(IOptions<Gw2ApiEndpoints> options, Gw2ApiClient gw2ApiClient)
        {
            _gw2ApiClient = gw2ApiClient;
            _gw2ApiEndpoints = options.Value;
        }

        public async Task<List<int>> FetchAllItemIdsFromApi(CancellationToken cancellationToken)
        {
            return await _gw2ApiClient.FetchAllItemIdsFromApi(cancellationToken,
                _gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint);
        }

        public async Task<List<Item>> FetchAllItemsFromApi(CancellationToken cancellationToken, int[] filter)
        {
            var ids = await _gw2ApiClient.FetchAllItemIdsFromApi(cancellationToken,
                _gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint);
            var chunkedItemIds = ids.Where(x => !filter.Contains(x)).Batch(150);
            var itemTasks = chunkedItemIds.Select(index => "?ids=" + string.Join(",", index.Select(x => x)))
                .Select(queryString =>
                    _gw2ApiClient.FetchAllItemsFromApi(cancellationToken,
                        _gw2ApiEndpoints.Gw2ApiItemEndpoint + queryString)).ToList();

            var resolvedItems = (await Task.WhenAll(itemTasks)).SelectMany(x => x).ToList();
            return resolvedItems;
        }

        public async Task<List<ItemPrice>> FetchAllItemPricesFromApi(CancellationToken cancellationToken, int[] filter)
        {
            var ids = await _gw2ApiClient.FetchAllItemIdsFromApi(cancellationToken,
                _gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint);
            var chunkedItemIds = ids.Where(x => !filter.Contains(x)).Batch(150);
            var tasks = chunkedItemIds.Select(index => _gw2ApiClient.FetchAllItemPricesFromApi(cancellationToken,
                    _gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + "?ids=" + string.Join(",", index.Select(x => x))))
                .ToList();

            return (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();
        }

        public async Task<List<Item>> FetchItemPricesFromApi(CancellationToken cancellationToken, int[] itemIds)
        {
            if (itemIds == null || itemIds.Length == 0)
            {
                return new List<Item>();
            }

            var queryString = "?ids=" + string.Join(",", string.Join(",", itemIds));
            var query = _gw2ApiEndpoints.Gw2ApiItemEndpoint + queryString;
            var priceQuery = _gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + queryString;
            var items = await _gw2ApiClient.FetchAllItemsFromApi(cancellationToken, query);
            var itemPrices = await _gw2ApiClient.FetchAllItemPricesFromApi(cancellationToken, priceQuery);

            items.ForEach(x =>
            {
                x.BuyData = itemPrices.FirstOrDefault(y => y.ItemPriceId == x.Id)?.BuyData;
                x.SellData = itemPrices.FirstOrDefault(y => y.ItemPriceId == x.Id)?.SellData;
            });

            return items;
        }
    }
}