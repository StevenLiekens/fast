using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fast_api.Config.AutoMapper
{
    public class ItemMapperProfile : Profile
    {
        public ItemMapperProfile()
        {
            CreateMap<Item, ItemDTO>()
                //.ForMember(x => x.PriceData.BuyPrice, y => y.MapFrom(y => y.BuyData.BuyPrice))
                .ForPath(x => x.PriceData.BuyPrice, y => y.MapFrom(y => y.BuyData.BuyPrice))
                .ForPath(x => x.PriceData.SellPrice, y => y.MapFrom(y => y.SellData.SellPrice));
            CreateMap<ItemTpData, PriceDataDTO>()
                .ForPath(x => x.BuyPrice, y => y.MapFrom(y => y.Buys.BuyPrice))
                .ForPath(x => x.SellPrice, y => y.MapFrom(y => y.Sells.SellPrice));
        }
    }
}
