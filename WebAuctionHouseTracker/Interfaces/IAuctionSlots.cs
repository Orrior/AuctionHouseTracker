using WebApplication1.Models.Web;

namespace WebApplication1.Interfaces;

public interface IAuctionSlots
{
    public List<AuctionSlot> GetCommodities();
    public List<AuctionSlot> GetNonCommodities(long realmId);
    public List<AuctionSlot> GetAllNonCommodities();
    public List<AuctionSlot> GetAll();

    public List<AuctionSlot> GetAll(long realmId);

    public IQueryable<AuctionSlot> GetAllAuctionSlots(long realmId);

    public List<AuctionSlot> PaginateById(int page, long realmId);

    public List<AuctionSlot> PaginateFilterByName(int page, long realmId, string itemName);

    public int GetPagesAmount(long realmId, string? itemName = "");

    public List<AuctionSlot> GetItemPriceHistory(long itemId, long realmId);
}