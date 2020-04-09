using Newtonsoft.Json;

namespace fast_api.Contracts.Models
{
    public class ItemSellInformation
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("unit_price")]
        public int UnitPrice { get; set; }
    }
}
