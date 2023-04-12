using AutoMapper;
using WebApplication1.Models;
using WebApplication1.Utils;

namespace WebApplication1.Mapping;

public class AuctionMappingProfile : Profile
{
    public AuctionMappingProfile()
    {
        //TODO! Map AuctionSlotCommodity to CommodityAuction etc.
        CreateMap<WowAuthenticatorRecords.AuctionSlotCommodity, CommodityAuction>()
            .ForMember(dest => dest.ItemId,
                b => b.MapFrom(
                    src => src.Item.Id));
        
        //TODO! Map AuctionSlotCommodity to CommodityAuction etc.
        CreateMap<WowAuthenticatorRecords.AuctionSlotNonCommodity, NonCommodityAuction>()
            .ForMember(dest => dest.ItemId,
                b => b.MapFrom(
                    src => src.Item.Id));

        //In favor of speed of database, commodity and non-commodity info items are separated to two different tables,
        //to avoid confusion we use two different classes with similar properties.
        CreateMap<WowAuthenticatorRecords.ItemInfo, CommodityInfo>()
            .ForMember(dest => dest.ItemClass, 
                b => b.MapFrom(
                    src => src.ItemClass.name))
            .ForMember(dest => dest.ItemSubClass, 
                b => b.MapFrom(
                    src => src.ItemSubclass.name));
        
        CreateMap<WowAuthenticatorRecords.ItemInfo, NonCommodityInfo>()
            .ForMember(dest => dest.ItemClass, 
                b => b.MapFrom(
                    src => src.ItemClass.name))
            .ForMember(dest => dest.ItemSubClass, 
                b => b.MapFrom(
                    src => src.ItemSubclass.name));
    }
}