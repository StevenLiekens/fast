using System.Linq;
using System.Security.Policy;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.Contracts.Models;
using fast_api.EntityFramework;
using Item = fast_api.Contracts.Models.Item;

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
            CreateMap<EntityFramework.Item, ItemDTO>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(src => src.ItemId))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(src => src.Name))
                .ForPath(x => x.PriceData.BuyPrice, cfg => cfg.MapFrom(src => src.Buy))
                .ForPath(x => x.PriceData.SellPrice, cfg => cfg.MapFrom(src => src.Sell));
            CreateMap<Item, EntityFramework.Item>()
                .ForMember(x => x.ItemId, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(src => src.Name))
                .ForMember(x => x.Buy, cfg => cfg.MapFrom(src => src.BuyData.BuyPrice))
                .ForMember(x => x.Sell, cfg => cfg.MapFrom(src => src.SellData.SellPrice));
            CreateMap<SelectionContainerItem, SelectionContainerItemDTO>()
                .ForMember(x => x.Amount, cfg => cfg.MapFrom(src => src.Amount))
                .ForMember(x => x.Guaranteed, cfg => cfg.MapFrom(src => src.Guaranteed))
                .ForMember(x => x.Id, cfg => cfg.MapFrom(src => src.ItemId))
                .ForMember(x => x.Currency, cfg => cfg.MapFrom(src => src.Currency))
                .ForMember(x => x.Type, cfg => cfg.MapFrom(src => src.ItemType));
            CreateMap<SelectionContainer, SelectionContainerDTO>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(src => src.SelectionContainerId))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(src => src.Name))
                .ForMember(x => x.Items, cfg => cfg.MapFrom(src => src.SelectionContainerItems))
                .ForPath(x => x.Price.BuyPrice, cfg => cfg.MapFrom(src => src.Buy))
                .ForPath(x => x.Price.SellPrice, cfg => cfg.MapFrom(src => src.Sell));
            CreateMap<CurrencyTradeCost, CurrencyTradeCostDTO>()
                .ForMember(x => x.Amount, cfg => cfg.MapFrom(src => src.Amount))
                .ForMember(x => x.Currency, cfg => cfg.MapFrom(src => src.Currency));
            CreateMap<CurrencyTrade, CurrencyTradeDTO>()
                .ForMember(x => x.CurrencyTradeId, cfg => cfg.MapFrom(src => src.CurrencyTradeId))
                .ForMember(x => x.Name, cfg => cfg.MapFrom(src => src.Name))
                .ForMember(x => x.Info, cfg => cfg.MapFrom(src => src.Info))
                .ForMember(x => x.Type, cfg => cfg.MapFrom(src => src.ItemType))
                .ForMember(x => x.ItemId, cfg => cfg.MapFrom(src => src.ItemId))
                .ForMember(x => x.ItemAmount, cfg => cfg.MapFrom(src => src.ItemAmount))
                .ForMember(x => x.SelectionContainerId, cfg => cfg.MapFrom(src => src.SelectionContainerId))
                .ForMember(x => x.SelectionContainerAmount, cfg => cfg.MapFrom(src => src.SelectionContainerAmount))
                .ForMember(x => x.CoinCost, cfg => cfg.MapFrom(src => src.CoinCost))
                .ForMember(x => x.CurrencyTradeCost, cfg => cfg.MapFrom(src => src.CurrencyTradeCost))
                .ForMember(x => x.Buy, cfg => cfg.MapFrom(src => src.Buy))
                .ForMember(x => x.Sell, cfg => cfg.MapFrom(src => src.Sell));
        }
    }
}
