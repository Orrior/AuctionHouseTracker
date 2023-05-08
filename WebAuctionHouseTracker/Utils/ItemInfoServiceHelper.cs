using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Repos;

namespace WebApplication1.Utils;

public static class ItemInfoServiceHelper
{
    public static void ScanCommodityInfo(ApplicationDbContext context, IAuctionRequests auctionRequests)
    {
        var commodityInfos = auctionRequests.GetCommodityInfos().Result;
        new CommodityInfos(context).AddOrUpdateRange(commodityInfos);
    }

    public static void ScanNonCommodityInfo(ApplicationDbContext context, IAuctionRequests auctionRequests, string realmId)
    {
        var nonCommodityInfos = auctionRequests.GetNonCommodityInfos(realmId).Result;
        new NonCommodityInfos(context).AddOrUpdateRange(nonCommodityInfos);
        
    }
}