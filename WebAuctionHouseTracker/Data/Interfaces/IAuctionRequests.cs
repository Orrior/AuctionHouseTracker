using WebApplication1.Utils;

namespace WebApplication1.Models;

public interface IAuctionRequests
{
    public Task<List<WowAuthenticatorRecords.AuctionSlotNonCommodity>> GetNonCommodities(string realmId);
    public Task<List<WowAuthenticatorRecords.AuctionSlotCommodity>> GetCommodities();
    public Task<List<WowAuthenticatorRecords.AuctionSlotNonCommodity>> GetCheapestNonCommodities(string realmId);
    public Task<List<WowAuthenticatorRecords.AuctionSlotCommodity>> GetCheapestCommodities();
    public Task<List<WowAuthenticatorRecords.ItemInfo>> GetItemInfos(List<long> itemIds);
    public WowAuthenticatorRecords.ItemInfo? GetItemInfo(long itemId);
    public Task<List<CommodityInfo>> GetCommodityInfos();
    public Task<List<NonCommodityInfo>> GetNonCommodityInfos(string realmId);
}