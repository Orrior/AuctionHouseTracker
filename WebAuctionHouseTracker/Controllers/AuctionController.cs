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
    public IActionResult Index(long realmId, int page)
    {
        Console.WriteLine($"realmId: {realmId}, page: {page}");

        var timer = DateTime.Now;
        _auction.PaginateById(0);
        Console.WriteLine($"TIME SPENT TO PAGINATE: {DateTime.Now - timer}");

        // List<AuctionSlot> result = _auction.GetAll(realmId);
        List<AuctionSlot> result = new List<AuctionSlot>();
        //TODO Paginator
        
        // return View(_auction.GetNonCommodities(realmId));
        return View(result);
    }
}