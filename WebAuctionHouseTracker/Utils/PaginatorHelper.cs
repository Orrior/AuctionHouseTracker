using WebApplication1.Models.Web;

namespace WebApplication1.Utils;

public class PaginatorHelper
{
    private readonly int _itemsInPage = 500;
    
    public List<AuctionSlot> PaginateById(int page, long realmId, IQueryable<AuctionSlot> paginateSlots)
    {
        var auctionSlots = paginateSlots
            .OrderBy(x => x.ItemId)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateByPrice(int page, long realmId, IQueryable<AuctionSlot> paginateSlots)
    {
        var auctionSlots = paginateSlots
            .OrderBy(x => x.Price)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateByQuantity(int page, long realmId, IQueryable<AuctionSlot> paginateSlots)
    {
        var auctionSlots = paginateSlots
            .OrderBy(x => x.Quantity)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }

    public List<AuctionSlot> PaginateByCategory(int page, long realmId, IQueryable<AuctionSlot> paginateSlots)
    {
        var auctionSlots = paginateSlots
            .OrderBy(x => x.Category)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateBySubcategory(int page, long realmId, IQueryable<AuctionSlot> paginateSlots)
    {
        var auctionSlots = paginateSlots
            .OrderBy(x => x.SubCategory)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateByPriceChange(int page, long realmId, IQueryable<AuctionSlot> paginateSlots)
    {
        throw new NotImplementedException("PriceChange Tracking is not yet implemented!");
    }
}