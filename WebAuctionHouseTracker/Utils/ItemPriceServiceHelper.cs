using AutoMapper;
using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Models.Auctions;

namespace WebApplication1.Utils;

public static class ItemPriceServiceHelper
{
    public static void ScanCommodityPrices(ApplicationDbContext context, IAuctionRequests auctionRequests, IMapper mapper, DateTime timestamp)
    {
        //scan commodities
        var commodities = auctionRequests.GetCheapestCommodities().Result
            .Select(mapper.Map<CommodityAuction>).ToList();
            
        commodities.ForEach(x => x.TimeStamp = timestamp);

        //save commodities
        context.CommodityAuctions.AddRange(commodities);
        context.SaveChanges();
    }

    public static void ScanNonCommodityPrices(ApplicationDbContext context, IAuctionRequests auctionRequests, IMapper mapper, DateTime timestamp, string realmId)
    {
        var nonCommodities = auctionRequests.GetCheapestNonCommodities(realmId).Result
            .Select(mapper.Map<NonCommodityAuction>).ToList();

        nonCommodities.ForEach(x => x.TimeStamp = timestamp);

        context.NonCommodityAuctions.AddRange(nonCommodities);
        context.SaveChanges();
    }
}