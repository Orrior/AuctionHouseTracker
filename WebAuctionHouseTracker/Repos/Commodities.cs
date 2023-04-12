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
}