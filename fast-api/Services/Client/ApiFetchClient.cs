using fast_api.Config;
using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace fast_api.Services.Client
{
    public class ApiFetchClient : IApiFetchClient
    {
        private readonly Gw2ApiEndpoints gw2ApiEndpoints;
        public ApiFetchClient(IOptions<Gw2ApiEndpoints> options)
        {
            gw2ApiEndpoints = options.Value;
        }
        //public async Task<List<ItemPrice>> FetchItemsFromApi()
        //{
        //    var items = new List<int>();
        //    string commerceURI;
        //    using (var client = new HttpClient())
        //    {
        //        using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiItemEndpoint))
        //        {
        //            var result = await response.Content.ReadAsStringAsync();
        //            items = JsonConvert.DeserializeObject<List<int>>(result);
        //            commerceURI = gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + "?ids=" + string.Join(",", items.Take(200));
        //        }
        //        using (var response = await client.GetAsync(commerceURI))
        //        {
        //            var result = await response.Content.ReadAsStringAsync();
        //            var json = JArray.Parse(result).ToString();
        //            return JsonConvert.DeserializeObject<List<ItemPrice>>(json);
        //        }
        //    }
        //}

        public async Task<List<Item>> FetchItemPricesFromApi(int[] itemIds)
        {
            using (var client = new HttpClient())
            {
                var items = new List<ItemPrice>();
                using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + "?ids=" + string.Join(",", itemIds)))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JArray.Parse(result).ToString();
                    items = JsonConvert.DeserializeObject<List<ItemPrice>>(json);
                }
                using ( var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiItemEndpoint + "?ids=" + string.Join(",", itemIds)))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JArray.Parse(result).ToString();
                    var itemsWithNames = JsonConvert.DeserializeObject<List<Item>>(json);
                    itemsWithNames.ForEach(x => x.PriceData = items.FirstOrDefault(y => y.ItemPriceId == x.Id).TpData);
                    return itemsWithNames;
                }
            }
        }

        public async Task<Item> FetchItemPriceFromApi(int id)
        {
            using (var client = new HttpClient())
            {
                var item = new ItemPrice();
                using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiCommercePricesEndpoint + "/" + id))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(result).ToString();
                    item = JsonConvert.DeserializeObject<ItemPrice>(json);
                }
                using (var response = await client.GetAsync(gw2ApiEndpoints.Gw2ApiItemEndpoint + "/" + id))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(result).ToString();
                    var itemWithName = JsonConvert.DeserializeObject<Item>(json);
                    itemWithName.PriceData = item.TpData;
                    return itemWithName;
                }
            }
        }
    }
}
