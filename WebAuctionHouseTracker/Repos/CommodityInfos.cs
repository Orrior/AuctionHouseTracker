using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Repos;

public class CommodityInfos
{
    private ApplicationDbContext _dbContext;

    public CommodityInfos(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async void AddOrUpdate(CommodityInfo commodityInfo)
    {
        var test = await _dbContext.CommodityInfos.FindAsync(commodityInfo.Id);
        
        if (test != null)
        {
            _dbContext.CommodityInfos.Update(commodityInfo);
        }
        else
        {
            _dbContext.CommodityInfos.Add(commodityInfo);
        }
    }

    public void AddOrUpdateRange(List<CommodityInfo> commodityInfoList)
    {
        // Use non-async version to prevent  
        int counter = 0;


        for (int i = 0; i < commodityInfoList.Count; i++)
        {
            var alreadyInDatabase = _dbContext.CommodityInfos.Any(x => x.Id == commodityInfoList[i].Id);

            if (alreadyInDatabase)
            {
                Console.WriteLine($"{commodityInfoList[i].Id} already in database, updating...");
                _dbContext.CommodityInfos.Update(commodityInfoList[i]);
            }
            else
            {
                _dbContext.CommodityInfos.Add(commodityInfoList[i]);
            }

            counter++;
        }
        _dbContext.SaveChanges();
    }
}