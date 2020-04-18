using System.Text.Json.Serialization;

namespace fast_api.Contracts.Models
{
    public class ItemTpData
    {
        [JsonPropertyName("buys")]
        public BuyData Buys { get; set; }
        [JsonPropertyName("sells")]
        public SellData Sells { get; set; }
    }

    public class BuyData
    {
        [JsonPropertyName("unit_price")]
        public int BuyPrice { get; set; }
        [JsonPropertyName("quantity")]
        public int BuyQuantity { get; set; }
    }

    public class SellData
    {
        [JsonPropertyName("unit_price")]
        public int SellPrice { get; set; }
        [JsonPropertyName("quantity")]
        public int SellQuantity { get; set; }
    }
}
