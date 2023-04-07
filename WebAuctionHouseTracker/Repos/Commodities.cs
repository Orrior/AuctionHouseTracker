using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Repos;

public class CommoditiesRepository
{
    private ApplicationDbContext _dbContext;

    public CommoditiesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async void Add(CommodityAuction commodityAuction)
    {
        //TODO! Add only unique objects.
        
        var test = _dbContext.CommodityAuctions;


        return;
        
    }
}