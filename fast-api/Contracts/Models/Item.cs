using Newtonsoft.Json;

namespace fast_api.Contracts.Models
{
    public class Item
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        public ItemSellInformation PriceData { get; set; }
    }
}
