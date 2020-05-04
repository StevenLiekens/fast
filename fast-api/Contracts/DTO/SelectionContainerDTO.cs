using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fast_api.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace fast_api.Contracts.DTO
{
    public class SelectionContainerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SelectionContainerItemDTO[] Items { get; set; }
        public PriceDataDTO Price { get; set; }
    }

    public class SelectionContainerItemDTO
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType Type { get; set; }

        public int Id { get; set; }
        public string Currency { get; set; }

        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }
}
