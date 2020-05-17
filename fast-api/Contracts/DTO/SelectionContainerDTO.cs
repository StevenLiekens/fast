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
        public int SelectionContainerId { get; set; }
        public string Name { get; set; }
        public SelectionContainerItemDTO[] Items { get; set; }
        public int Buy { get; set; }
        public int Sell { get; set; }
    }

    public class SelectionContainerItemDTO
    {
        public int ItemId { get; set; }
        
        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }
}
