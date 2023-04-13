using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;

namespace WebApplication1.Models;

public class NonCommodityInfo : IBasicAuction.IAuctionInfo
{
    //TODO!!! Maybe long, not int?
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string ItemClass { get; set; } = "";
    public string ItemSubClass { get; set; } = "";
}

public class NonCommodityAuction : IBasicAuction.IAuctionSlot
{
    public long RealmId { get; set; }
    public long ItemId { get; set; }
    public int Quantity { get; set; }
    public long Buyout { get; set; }
    public long Bid { get; set; }
    public string TimeLeft { get; set; } = "";
    public DateTime TimeStamp { get; set; } =  DateTime.Now;
}