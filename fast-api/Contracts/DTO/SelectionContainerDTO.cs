using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public int Id { get; set; }
        public bool Guaranteed { get; set; }
        public int Amount { get; set; }
    }
}
