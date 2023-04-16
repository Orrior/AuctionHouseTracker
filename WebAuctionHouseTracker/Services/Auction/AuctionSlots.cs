using System.Text.Json;
using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Models.Web;
using WebApplication1.Repos;

namespace WebApplication1.Services.Auction;

public class AuctionSlots : IAuctionSlots
{
    private ApplicationDbContext _context;
    private readonly int _itemsInPage = 500;
    
    public AuctionSlots(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<AuctionSlot> GetAllAuctionSlots(long realmId)
    {
        var timestamp = _context.CommodityAuctions.Max(x => x.TimeStamp);
        var commodities = _context.CommodityAuctions
            .Where(x => x.TimeStamp == timestamp);
        var nonCommodities = _context.NonCommodityAuctions
            .Where(x => x.RealmId == realmId)
            .Where(x => x.TimeStamp == timestamp);

        //STEP 1 - Join CommodityAuction and CommodityInfos -> ComSlot
        var comSlot = commodities.Join(
            _context.CommodityInfos,
            x => x.ItemId,
            y => y.Id,
            (auction, info) => new AuctionSlot
            {
                ItemId = auction.ItemId,
                Name = info.Name,
                RealmId = -1,
                IsCommodity = true,
                Price = auction.UnitPrice,
                Bid = 0,
                Quantity = auction.Quantity,
                Category = info.ItemClass,
                SubCategory = info.ItemSubClass,
                TimeLeft = auction.TimeLeft,
                TimeStamp = auction.TimeStamp
            });
        //STEP 2 - Join NonComAuction and NonComInfos -> NonComSlot
        var nonComSlot = nonCommodities.Join(
            _context.NonCommodityInfos,
            x => x.ItemId,
            y => y.Id,
            (auction, info) => new AuctionSlot
            {
                ItemId = auction.ItemId,
                Name = info.Name,
                RealmId = auction.RealmId,
                IsCommodity = false,
                Price = auction.Buyout,
                Bid = auction.Bid,
                Quantity = auction.Quantity,
                Category = info.ItemClass,
                SubCategory = info.ItemSubClass,
                TimeLeft = auction.TimeLeft,
                TimeStamp = auction.TimeStamp
            });

        return comSlot.Concat(nonComSlot);
    }
    
    public List<AuctionSlot> PaginateById(int page, long realmId)
    {
        var auctionSlots = GetAllAuctionSlots(realmId)
            .OrderBy(x => x.ItemId)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateFilterByName(int page, long realmId, string itemName)
    {
        var auctionSlots = GetAllAuctionSlots(realmId)
            .Where(x => x.Name.ToLower().Contains(itemName.ToLower()))
            .OrderBy(x => x.ItemId)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateByPrice(int page, long realmId)
    {
        var auctionSlots = GetAllAuctionSlots(realmId)
            .OrderBy(x => x.Price)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateByQuantity(int page, long realmId)
    {
        var auctionSlots = GetAllAuctionSlots(realmId)
            .OrderBy(x => x.Quantity)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }

    public List<AuctionSlot> PaginateByCategory(int page, long realmId)
    {
        var auctionSlots = GetAllAuctionSlots(realmId)
            .OrderBy(x => x.Category)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }
    
    public List<AuctionSlot> PaginateBySubcategory(int page, long realmId)
    {
        var auctionSlots = GetAllAuctionSlots(realmId)
            .OrderBy(x => x.SubCategory)
            .Skip(page * _itemsInPage)
            .Take(_itemsInPage);
        return auctionSlots.ToList();
    }

    public List<AuctionSlot> GetItemPriceHistory(long itemId, long realmId)
    {
        List<AuctionSlot> auctionSlots = new List<AuctionSlot>();
        
        if (_context.CommodityInfos.Any(x => x.Id == itemId))
        {
            var commodityAuctions = _context.CommodityAuctions
                .Where(x => x.ItemId == itemId).OrderBy(x => x.TimeStamp);
            
            var comSlot = commodityAuctions.Join(
                _context.CommodityInfos,
                x => x.ItemId,
                y => y.Id,
                (auction, info) => new AuctionSlot
                {
                    ItemId = auction.ItemId,
                    Name = info.Name,
                    RealmId = -1,
                    IsCommodity = true,
                    Price = auction.UnitPrice,
                    Bid = 0,
                    Quantity = auction.Quantity,
                    Category = info.ItemClass,
                    SubCategory = info.ItemSubClass,
                    TimeLeft = auction.TimeLeft,
                    TimeStamp = auction.TimeStamp
                });

            auctionSlots = comSlot.ToList();
        }
        else if (_context.NonCommodityInfos.Any(x => x.Id == itemId))
        {
            var nonCommodityAuctions = _context.NonCommodityAuctions
                .Where(x => x.ItemId == itemId && x.RealmId == realmId);
            
            var nonComSlot = nonCommodityAuctions.Join(
                _context.NonCommodityInfos,
                x => x.ItemId,
                y => y.Id,
                (auction, info) => new AuctionSlot
                {
                    ItemId = auction.ItemId,
                    Name = info.Name,
                    RealmId = auction.RealmId,
                    IsCommodity = false,
                    Price = auction.Buyout,
                    Bid = auction.Bid,
                    Quantity = auction.Quantity,
                    Category = info.ItemClass,
                    SubCategory = info.ItemSubClass,
                    TimeLeft = auction.TimeLeft,
                    TimeStamp = auction.TimeStamp
                });

            auctionSlots = nonComSlot.ToList();
        }

        return auctionSlots;
    }
    
    public int GetPagesAmount(long realmId, string? itemName = "")
    {

        var aSlot = GetAllAuctionSlots(realmId);

        if (!String.IsNullOrEmpty(itemName))
        {
            aSlot = aSlot.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }
        
        var items = aSlot.Count();
        var pages = items / _itemsInPage;
        
        if (items % _itemsInPage != 0)
        {
            pages++;
        }

        return pages;
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