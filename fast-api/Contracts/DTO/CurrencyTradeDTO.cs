using fast_api.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace fast_api.Contracts.DTO
{
    public class CurrencyTradeDTO
    {
        public int CurrencyTradeId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public CurrencyTradeCostDTO[] CurrencyTradeCost { get; set; }
        public int CoinCost { get; set; }

        public string Type { get; set; }

        public int ItemId { get; set; }
        public int SelectionContainerId { get; set; }
        public int ContainerId { get; set; }
        public int CategoryId { get; set; }
        
        public int Amount { get; set; }

        public int Buy { get; set; }
        public int Sell { get; set; }
    }

    public class CurrencyTradeCostDTO
    {
        public string Currency { get; set; }
        public int Amount { get; set; }
    }
}