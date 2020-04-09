using Newtonsoft.Json;

namespace fast_api.Contracts.Models
{
    public class ItemPrice
    {
        [JsonProperty("id")]
        public int ItemPriceId { get; set; }
        [JsonProperty("sells")]
        public ItemSellInformation TpData { get; set; }
    }
}
