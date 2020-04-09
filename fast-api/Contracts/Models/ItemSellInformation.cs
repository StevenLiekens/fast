using System.Text.Json.Serialization;

namespace fast_api.Contracts.Models
{
    public class ItemSellInformation
    {
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("unit_price")]
        public int UnitPrice { get; set; }
    }
}
