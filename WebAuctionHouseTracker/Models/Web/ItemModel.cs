namespace WebApplication1.Models.Web;

public class ItemModel
{
    public List<AuctionSlot> AuctionSlots { get; set; }
    public long RealmId { get; set; }
    public long ItemId { get; set; }
}