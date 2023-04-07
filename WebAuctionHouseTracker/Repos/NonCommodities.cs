using WebApplication1.Migrations;

namespace WebApplication1.Repos;

public class NonCommodities
{
    private ApplicationDbContext _dbContext;

    public NonCommodities(ApplicationDbContext context)
    {
        _dbContext = context;
    }
}