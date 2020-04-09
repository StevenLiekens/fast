using System.Text.Json.Serialization;

namespace fast_api.Contracts.Models
{
    public class Item
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public ItemSellInformation PriceData { get; set; }
    }
}
