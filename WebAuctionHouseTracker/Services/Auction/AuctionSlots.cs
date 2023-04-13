using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Models.Web;
using WebApplication1.Repos;

namespace WebApplication1.Services.Auction;

public class AuctionSlots : IAuctionSlots
{
    private ApplicationDbContext _context;

    public AuctionSlots(ApplicationDbContext context)
    {
        _context = context;
    }

    //TODO Add RealmId!
    public List<AuctionSlot> PaginateById(int page)
    {
        var commodities = _context.CommodityAuctions;
        Console.WriteLine(commodities.Count());
        commodities.GroupBy(x => x.ItemId);
        Console.WriteLine(commodities.Count());

        // var nonCommodities = _context.NonCommodityAuctions;
        //
        // //STEP 1 - Join CommodityAuction and CommodityInfos -> ComSlot
        // var comSlot = _context.CommodityAuctions.Join(
        //     _context.CommodityInfos,
        //     x => x.ItemId,
        //     y => y.Id,
        //     (auction, info) => new AuctionSlot
        //     {
        //         ItemId = auction.ItemId,
        //         Name = info.Name,
        //         RealmId = -1,
        //         IsCommodity = true,
        //         Price = auction.UnitPrice,
        //         Bid = 0,
        //         Quantity = auction.Quantity,
        //         Category = info.ItemClass,
        //         SubCategory = info.ItemSubClass,
        //         TimeLeft = auction.TimeLeft,
        //         TimeStamp = auction.TimeStamp
        //     });
        //
        //
        // //STEP 2 - Join NonComAuction and NonComInfos -> NonComSlot
        // var nonComSlot = _context.NonCommodityAuctions.Join(
        //     _context.NonCommodityInfos,
        //     x => x.ItemId,
        //     y => y.Id,
        //     (auction, info) => new AuctionSlot
        //     {
        //         ItemId = auction.ItemId,
        //         Name = info.Name,
        //         RealmId = auction.RealmId,
        //         IsCommodity = false,
        //         Price = auction.Buyout,
        //         Bid = auction.Bid,
        //         Quantity = auction.Quantity,
        //         Category = info.ItemClass,
        //         SubCategory = info.ItemSubClass,
        //         TimeLeft = auction.TimeLeft,
        //         TimeStamp = auction.TimeStamp
        //     });
        //
        // //Step 3 - Join ComSlot and NonComSlot
        // var auctionSlots = comSlot.Concat(nonComSlot).OrderBy(x => x.ItemId).Skip(page * 500).Take(500);
        List<AuctionSlot> auctionSlots = new List<AuctionSlot>();
        return auctionSlots.ToList();
    }

    public List<AuctionSlot> GetCommodities()
    {
        var comAuctions = _context.CommodityAuctions.ToList();

        List<AuctionSlot> resultList = new List<AuctionSlot>();
        
        for (int i = 0; i < comAuctions.Count(); i++)
        {
            var item = comAuctions[i];

            var info = _context.CommodityInfos.Find(item.ItemId);

            if (info != null)
            {
                resultList.Add(new AuctionSlot(item, info));
            }
        }

        return resultList;
    }

    public List<AuctionSlot> GetAllNonCommodities()
    {
        var comAuctions = _context
            .NonCommodityAuctions.ToList();

        List<AuctionSlot> resultList = new List<AuctionSlot>();
        
        for (int i = 0; i < comAuctions.Count(); i++)
        {
            var item = comAuctions[i];

            var info = _context.NonCommodityInfos.Find(item.ItemId);

            if (info != null)
            {
                resultList.Add(new AuctionSlot(item, info));
            }
        }
        return resultList;
    }
    
    public List<AuctionSlot> GetNonCommodities(long realmId)
    // Get non-commodities for specific ID.
    {
        var comAuctions = _context
            .NonCommodityAuctions.Where(x => x.RealmId == realmId).ToList();

        List<AuctionSlot> resultList = new List<AuctionSlot>();
        
        for (int i = 0; i < comAuctions.Count(); i++)
        {
            var item = comAuctions[i];

            var info = _context.NonCommodityInfos.Find(item.ItemId);

            if (info != null)
            {
                resultList.Add(new AuctionSlot(item, info));
            }
        }
        return resultList;
    }

    public List<AuctionSlot> GetAll(long realmId)
    {
        List<AuctionSlot> auctionSlots = GetCommodities();
        auctionSlots.AddRange(GetNonCommodities(realmId));
        
        return auctionSlots;
    }

    public List<AuctionSlot> GetAll()
    {
        List<AuctionSlot> auctionSlots = GetCommodities();
        auctionSlots.AddRange(GetAllNonCommodities());
        
        return auctionSlots;
    }
}