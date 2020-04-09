
using System.Text.Json.Serialization;

namespace fast_api.Contracts.Models
{
    public class ItemPrice
    {
        [JsonPropertyName("id")]
        public int ItemPriceId { get; set; }
        [JsonPropertyName("sells")]
        public ItemSellInformation TpData { get; set; }
    }
}
