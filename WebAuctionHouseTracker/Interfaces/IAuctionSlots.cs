using WebApplication1.Models.Web;

namespace WebApplication1.Interfaces;

public interface IAuctionSlots
{
    public List<AuctionSlot> GetCommodities();
    public List<AuctionSlot> GetNonCommodities(long realmId);
    public List<AuctionSlot> GetAllNonCommodities();
    public List<AuctionSlot> GetAll();

    public List<AuctionSlot> GetAll(long realmId);

    public List<AuctionSlot> PaginateById(int page);
}