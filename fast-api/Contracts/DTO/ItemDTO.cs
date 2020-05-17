using System.Text.Json.Serialization;

namespace fast_api.Contracts.DTO
{
    public class ItemDTO
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public int Buy { get; set; }
        public int Sell { get; set; }
    }
}
