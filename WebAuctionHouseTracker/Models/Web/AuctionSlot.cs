namespace WebApplication1.Models.Web;

public class AuctionSlot
{
    public long ItemId { get; set; }
    public string Name { get; set; }

    public long RealmId { get; set; }
    public bool IsCommodity { get; set; }
    
    public long Price { get; set; }
    public long Bid { get; set; }
    
    public long Quantity { get; set; }
    
    public string Category { get; set; }
    public string SubCategory { get; set; }

    public string TimeLeft { get; set; }
    
    public DateTime TimeStamp { get; set; }

    public AuctionSlot(NonCommodityAuction nonComAuction, NonCommodityInfo nonComInfo)
    {
        ItemId = nonComInfo.Id;
        Name = nonComInfo.Name;

        RealmId = nonComAuction.RealmId;
        IsCommodity = false;

        Price = nonComAuction.Buyout;
        Bid = nonComAuction.Bid;
        Quantity = nonComAuction.Quantity;

        Category = nonComInfo.ItemClass;
        SubCategory = nonComInfo.ItemSubClass;

        TimeLeft = nonComAuction.TimeLeft;
        TimeStamp = nonComAuction.TimeStamp;
    }

    public AuctionSlot(CommodityAuction comAuction, CommodityInfo comInfo)
    {
        ItemId = comInfo.Id;
        Name = comInfo.Name;

        RealmId = -1;
        IsCommodity = true;

        Price = comAuction.UnitPrice;
        Quantity = comAuction.Quantity;
        
        Category = comInfo.ItemClass;
        SubCategory = comInfo.ItemSubClass;

        TimeLeft = comAuction.TimeLeft;
        TimeStamp = comAuction.TimeStamp;
    }

    public AuctionSlot()
    {

    }
}