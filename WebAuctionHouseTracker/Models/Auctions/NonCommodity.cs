using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public class NonCommodityInfo : IBasicAuction.IAuctionInfo
{
    //TODO!!! Maybe long, not int?
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string ItemClass { get; set; } = "";
    public string ItemSubClass { get; set; } = "";
}

public class NonCommodityAuction : IBasicAuction.IAuctionSlot
{
    //TODO!!! Maybe long, not int?
    public int RealmId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public int Buyout { get; set; }
    public int Bid { get; set; }
    public string TimeLeft { get; set; } = "";
    public DateTime TimeStamp { get; set; }
}