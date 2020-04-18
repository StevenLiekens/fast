using System.Text.Json.Serialization;

namespace fast_api.Contracts.DTO
{
    public class PriceDataDTO
    {
        [JsonPropertyName("buy")]
        public string BuyPrice { get; set; }
        [JsonPropertyName("sell")]
        public string SellPrice { get; set; }
    }
}
