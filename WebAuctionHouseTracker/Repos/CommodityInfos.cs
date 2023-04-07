using WebApplication1.Migrations;

namespace WebApplication1.Repos;

public class CommodityInfos
{
    private ApplicationDbContext _dbContext;

    public CommodityInfos(ApplicationDbContext context)
    {
        _dbContext = context;
    }
}