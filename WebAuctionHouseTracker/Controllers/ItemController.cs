using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models.Web;

namespace WebApplication1.Controllers;

public class ItemController : Controller
{

    private readonly IAuctionSlots _auctionSlots;
    
    public ItemController(IAuctionSlots auctionSlots)
    {
        _auctionSlots = auctionSlots;
    }
    
    // GET
    public IActionResult Index(long itemId, long realmId)
    {
        List<AuctionSlot> auctionSlots = _auctionSlots.GetItemPriceHistory(itemId, realmId);
        return View(new ItemModel(){AuctionSlots = auctionSlots, ItemId = itemId, RealmId = realmId});
    }
}