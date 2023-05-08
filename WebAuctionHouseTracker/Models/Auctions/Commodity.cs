using WebApplication1.Interfaces;

namespace WebApplication1.Models.Auctions;

public class CommodityInfo : BasicAuction.IAuctionInfo
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string ItemClass { get; set; } = "";
    public string ItemSubClass { get; set; } = "";
}

public class CommodityAuction : BasicAuction.IAuctionSlot
{
    public long ItemId { get; set; }
    public int Quantity { get; set; }
    public long UnitPrice { get; set; }
    public string TimeLeft { get; set; } = "";
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}