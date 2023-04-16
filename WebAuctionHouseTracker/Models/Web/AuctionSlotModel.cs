namespace WebApplication1.Models.Web;

public class AuctionSlotModel
{
    public List<AuctionSlot> AuctionSlots { get; set; }
    public long totalPages { get; set; }
    
    public int currentPage { get; set; }
    
    public long realmId { get; set; }
    
    public string? itemName { get; set; }
}