using WebApplication1.Migrations;

namespace WebApplication1.Repos;

public class NonCommodityInfos
{
    private ApplicationDbContext _dbContext;

    public NonCommodityInfos(ApplicationDbContext context)
    {
        _dbContext = context;
    }
}