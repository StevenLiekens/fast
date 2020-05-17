using System.Linq;
using System.Security.Policy;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.Contracts.Models;
using fast_api.EntityFramework;
using fast_api.EntityFramework.Entities;
using Item = fast_api.Contracts.Models.Item;

namespace fast_api.Config.AutoMapper
{
    public class ItemMapperProfile : Profile
    {
        public ItemMapperProfile()
        {
            CreateMap<Item, ItemDTO>()
                .ForPath(x => x.Buy, y => y.MapFrom(z => z.BuyData.BuyPrice))
                .ForPath(x => x.Sell, y => y.MapFrom(z => z.SellData.SellPrice));
            CreateMap<ItemTpData, PriceDataDTO>()
                .ForPath(x => x.BuyPrice, y => y.MapFrom(z => z.Buys.BuyPrice))
                .ForPath(x => x.SellPrice, y => y.MapFrom(z => z.Sells.SellPrice));
            CreateMap<EntityFramework.Entities.Item, ItemDTO>();
            CreateMap<Item, EntityFramework.Entities.Item>()
                .ForMember(x => x.ItemId, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(x => x.Buy, cfg => cfg.MapFrom(src => src.BuyData.BuyPrice))
                .ForMember(x => x.Sell, cfg => cfg.MapFrom(src => src.SellData.SellPrice));
            CreateMap<SelectionContainerItem, SelectionContainerItemDTO>();
            CreateMap<SelectionContainer, SelectionContainerDTO>()
                .ForMember(x => x.Items, cfg => cfg.MapFrom(src => src.SelectionContainerItem));
            CreateMap<CurrencyTradeCost, CurrencyTradeCostDTO>();
            CreateMap<CurrencyTrade, CurrencyTradeDTO>()
                .ForMember(x => x.Type, cfg => cfg.MapFrom(src => src.ItemType));
        }
    }
}
