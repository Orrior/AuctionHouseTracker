using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public class CommodityInfo : IBasicAuction.IAuctionInfo
{
    //TODO!!! Maybe long, not int?
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string ItemClass { get; set; } = "";
    public string ItemSubClass { get; set; } = "";
}

public class CommodityAuction : IBasicAuction.IAuctionSlot
{
    //TODO!!! Maybe long, not int?
    public long ItemId { get; set; }
    public int Quantity { get; set; }
    public long UnitPrice { get; set; }
    public string TimeLeft { get; set; } = "";
    //TODO! Creation condition if initialized first time.
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}