using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Models.Auctions;

namespace WebApplication1.Repos;

public class NonCommodityInfos
{
    private ApplicationDbContext _dbContext;

    public NonCommodityInfos(ApplicationDbContext context)
    {
        _dbContext = context;
    }
    
    public async void AddOrUpdate(NonCommodityInfo nonCommodityInfo)
    {
        var test = await _dbContext.NonCommodityInfos.FindAsync(nonCommodityInfo.Id);
        
        if (test != null)
        {
            _dbContext.NonCommodityInfos.Update(nonCommodityInfo);
        }
        else
        {
            _dbContext.NonCommodityInfos.Add(nonCommodityInfo);
        }
    }
    
    public void AddOrUpdateRange(List<NonCommodityInfo> nonCommodityInfoList)
    {
        for (int i = 0; i < nonCommodityInfoList.Count; i++)
        {
            var alreadyInDatabase = _dbContext.NonCommodityInfos.Any(x => x.Id == nonCommodityInfoList[i].Id);
            //TODO!!! Rework, try to check by list if objects already in database...
            
            if (alreadyInDatabase)
            {
                Console.WriteLine($"{nonCommodityInfoList[i].Id} already in database, updating...");
                _dbContext.NonCommodityInfos.Update(nonCommodityInfoList[i]);
            }
            else
            {
                _dbContext.NonCommodityInfos.Add(nonCommodityInfoList[i]);
            }
        }
        
        _dbContext.SaveChanges();
    }
}