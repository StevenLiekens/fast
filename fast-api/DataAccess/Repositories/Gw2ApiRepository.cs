using fast_api.Config;
using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using fast_api.Extensions;
using fast_api.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace fast_api.DataAccess.Repositories
{
    public class Gw2ApiRepository : IGw2ApiRepository
    {
        private readonly Gw2ApiEndpoints gw2ApiEndpoints;
        private readonly Gw2ApiClientFactory _gw2ApiClientFactory;

        public Gw2ApiRepository(IOptions<Gw2ApiEndpoints> options, Gw2ApiClientFactory gw2ApiClientFactory)
        {
            gw2ApiEndpoints = options.Value;
            _gw2ApiClientFactory = gw2ApiClientFactory;
        }

        public async Task<List<int>> FetchAllItemIdsFromApi(CancellationToken cancellationToken)
        {
            var apiClient = _gw2ApiClientFactory.Create();
            return await apiClient.FetchAllItemIdsFromApi(cancellationToken, gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint);
        }

        public async Task<List<Item>> FetchAllItemsFromApi(CancellationToken cancellationToken, int[] filter)
        {
            var itemTasks = new List<Task<List<Item>>>();
            var priceTasks = new List<Task<List<ItemPrice>>>();
            var apiClient = _gw2ApiClientFactory.Create();
            IEnumerable<IEnumerable<int>> chunkedItemIds;
            List<Item> items = new List<Item>();
            var ids = await apiClient.FetchAllItemIdsFromApi(cancellationToken, gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint);
            chunkedItemIds = ids.Where(x => !filter.Contains(x)).Chunks(150);
            foreach (var index in chunkedItemIds)
            {
                var queryString = "?ids=" + string.Join(",", index.Select(x => x));
                itemTasks.Add(apiClient.FetchAllItemsFromApi(cancellationToken, gw2ApiEndpoints.Gw2ApiItemEndpoint + queryString));
                //priceTasks.Add(apiClient.FetchAllItemPricesFromApi(cancellationToken, gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + queryString));
            }

            var resolvedItems = (await Task.WhenAll(itemTasks)).SelectMany(x => x).ToList();
            //var resolvedPrices = (await Task.WhenAll(priceTasks)).SelectMany(x => x).ToList();
            //resolvedItems.ForEach(x => x.PriceData = resolvedPrices.FirstOrDefault(y => y.ItemPriceId == x.Id).TpData);
            return resolvedItems;
        }

        public async Task<List<ItemPrice>> FetchAllItemPricesFromApi(CancellationToken cancellationToken, int[] filter)
        {
            var tasks = new List<Task<List<ItemPrice>>>();
            var apiClient = _gw2ApiClientFactory.Create();
            IEnumerable<IEnumerable<int>> chunkedItemIds;
            List<ItemPrice> items = new List<ItemPrice>();
            var ids = await apiClient.FetchAllItemIdsFromApi(cancellationToken, gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint);
            chunkedItemIds = ids.Where(x => !filter.Contains(x)).Chunks(150);
            foreach (var index in chunkedItemIds)
            {
                tasks.Add(apiClient.FetchAllItemPricesFromApi(cancellationToken, gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + "?ids=" + string.Join(",", index.Select(x => x))));
            }

            return (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();
        }

        public async Task<List<Item>> FetchItemPricesFromApi(CancellationToken cancellationToken, int[] itemIds)
        {
            //using (var client = new HttpClient())
            //{
            //    var items = new List<ItemPrice>();
            //    using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + "?ids=" + string.Join(",", itemIds)))
            //    {
            //        var result = await response.Content.ReadAsStringAsync();
            //        var json = JArray.Parse(result).ToString();
            //        items = JsonConvert.DeserializeObject<List<ItemPrice>>(json);
            //    }
            //    using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiItemEndpoint + "?ids=" + string.Join(",", itemIds)))
            //    {
            //        var result = await response.Content.ReadAsStringAsync();
            //        var json = JArray.Parse(result).ToString();
            //        var itemsWithNames = JsonConvert.DeserializeObject<List<Item>>(json);
            //        itemsWithNames.ForEach(x => x.PriceData = items.FirstOrDefault(y => y.ItemPriceId == x.Id).TpData);
            //        return itemsWithNames;
            //    }
            //}
            if (itemIds == null || itemIds.Length == 0)
            {
                return new List<Item>();
            }
            var queryString = "?ids=" + string.Join(",", string.Join(",", itemIds));
            var apiClient = _gw2ApiClientFactory.Create();
            var query = gw2ApiEndpoints.Gw2ApiItemEndpoint + queryString;
            var priceQuery = gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + queryString;
            var items = await apiClient.FetchAllItemsFromApi(cancellationToken, query);
            var itemPrices = await apiClient.FetchAllItemPricesFromApi(cancellationToken, priceQuery);

            items.ForEach(x =>
                {
                    x.BuyData = itemPrices.FirstOrDefault(y => y.ItemPriceId == x.Id).BuyData;
                    x.SellData = itemPrices.FirstOrDefault(y => y.ItemPriceId == x.Id).SellData;
                });

            return items;
        }

        //public async Task<Item> FetchItemPriceFromApi(int id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var item = new ItemPrice();
        //        using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + "/" + id))
        //        {
        //            var result = await response.Content.ReadAsStringAsync();
        //            var json = JObject.Parse(result).ToString();
        //            item = JsonConvert.DeserializeObject<ItemPrice>(json);
        //        }
        //        using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiItemEndpoint + "/" + id))
        //        {
        //            var result = await response.Content.ReadAsStringAsync();
        //            var json = JObject.Parse(result).ToString();
        //            var itemWithName = JsonConvert.DeserializeObject<Item>(json);
        //            itemWithName.PriceData = item.TpData;
        //            return itemWithName;
        //        }
        //    }
        //}
    }
}
