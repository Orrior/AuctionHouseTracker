using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Models.Web;
using WebApplication1.Repos;

namespace WebApplication1.Controllers;

public class AuctionController : Controller
{
    private IAuctionSlots _auction;

    public AuctionController(IAuctionSlots auctionSlots)
    {
        _auction = auctionSlots;
    }
    
    // GET
    public IActionResult Index(long realmId, int page, string? itemName)
    {
        if (itemName != "" && itemName != null)
        {
            return View(new AuctionSlotModel()
            {
                AuctionSlots = _auction.PaginateFilterByName(page, realmId, itemName),
                totalPages = _auction.GetPagesAmount(realmId, itemName),
                currentPage = page,
                realmId = realmId,
                itemName = itemName
            });


        }
        
        var timer = DateTime.Now;
        List<AuctionSlot> result = _auction.PaginateById(page, realmId);

        // return View(_auction.GetNonCommodities(realmId));
        return View(new AuctionSlotModel()
        {
            AuctionSlots = result,
            totalPages = _auction.GetPagesAmount(realmId),
            currentPage = page,
            realmId = realmId,
            itemName = itemName
        });
    }
}