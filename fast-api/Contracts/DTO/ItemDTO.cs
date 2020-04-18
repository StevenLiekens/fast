using System.Text.Json.Serialization;

namespace fast_api.Contracts.DTO
{
    public class ItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public PriceDataDTO PriceData { get; set; }
    }
}
