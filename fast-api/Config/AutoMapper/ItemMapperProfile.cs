using System.Linq;
using System.Security.Policy;
using AutoMapper;
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
            CreateMap<Item, EntityFramework.Entities.Item>()
                .ForMember(x => x.ItemId, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(x => x.Buy, cfg => cfg.MapFrom(src => src.BuyData.BuyPrice))
                .ForMember(x => x.Sell, cfg => cfg.MapFrom(src => src.SellData.SellPrice));
        }
    }
}
