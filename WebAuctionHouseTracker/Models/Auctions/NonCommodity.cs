using WebApplication1.Interfaces;

namespace WebApplication1.Models.Auctions;

public class NonCommodityInfo : BasicAuction.IAuctionInfo
{
    //TODO!!! Maybe long, not int?
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string ItemClass { get; set; } = "";
    public string ItemSubClass { get; set; } = "";
}

public class NonCommodityAuction : BasicAuction.IAuctionSlot
{
    public long RealmId { get; set; }
    public long ItemId { get; set; }
    public int Quantity { get; set; }
    public long Buyout { get; set; }
    public long Bid { get; set; }
    public string TimeLeft { get; set; } = "";
    public DateTime TimeStamp { get; set; } =  DateTime.Now;
}