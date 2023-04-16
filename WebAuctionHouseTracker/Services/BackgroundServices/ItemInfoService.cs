using System.Text.Json;
using AutoMapper;
using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Repos;
using WebApplication1.Utils;
using static WebApplication1.Utils.WowAuthenticatorRecords;

namespace WebApplication1.Services.BackgroundServices;

public class ItemInfoService : BackgroundService
{
    private readonly ILogger<ItemInfoService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    private string _region;
    private string[] _realms;
    private string _locale;
    
    private string token = "";
    private readonly IAuctionRequests _auctionRequests;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {jobName}", nameof(ItemPriceRequestService));
        while (!stoppingToken.IsCancellationRequested)
        {
            var timer1 = DateTime.Now;
            _logger.LogInformation("Starting info lists update... \r\n" +
                                   $"Region:{_region},Realms:[{string.Join(',', _realms)}],Locale:{_locale}");

            //Create scope and DB
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            //TODO!!! should we combine GetCommodityInfos and AddOrUpdateRange in one method so it will instantly add item in database instead of memory bloating?.
            // Scan all commodity items and add them to DB
             _logger.LogInformation("Start scanning commodities...");
             var commodityInfos = await _auctionRequests.GetCommodityInfos();
             var timer2 = DateTime.Now;
             _logger.LogInformation("Start saving commodities items info...");
            
             new CommodityInfos(context).AddOrUpdateRange(commodityInfos);
             var timer3 = DateTime.Now;
             _logger.LogInformation($"TOTAL COMINFOS IN DATABASE:{context.CommodityInfos.Count()}");
             _logger.LogInformation("Commodities items are successfully saved!");
            
             commodityInfos.Clear(); // make list eligible for GC.
             commodityInfos.TrimExcess(); // reallocates list memory.
            
             //TODO! DateTime.Now;e's no realms mentioned in config this code still will go and not make any warn?
             //TODO! otherwise, add check in the beginning of the task so it will check whether realms assigned in config or not.
             _logger.LogInformation("Start scanning non-commodities...");
             var nonCommodityInfos = await _auctionRequests.GetNonCommodityInfos(_realms[0]);
             _logger.LogInformation("Non-commodities scanning is finished!");
             var timer4 = DateTime.Now;
             _logger.LogInformation("Start saving non-commodities items info...");
             new NonCommodityInfos(context).AddOrUpdateRange(nonCommodityInfos);
             _logger.LogInformation($"TOTAL NONCOMINFOS IN DATABASE:{context.NonCommodityInfos.Count()}");
             _logger.LogInformation("Non-commodities items are successfully saved!");
            
             nonCommodityInfos.Clear(); // make list eligible for GC.
             nonCommodityInfos.TrimExcess(); // reallocates list memory.
            
             var timer5 = DateTime.Now;
             _logger.LogInformation("===TOTAL SAVED=== " + 
                                    $"\r\n commodities:{context.CommodityInfos.Count()} items" + 
                                    $"\r\n non-commodities: {context.NonCommodityInfos.Count()} items " +
                                     "\r\n ===TIME ELAPSED=== " +
                                    $"\r\n Getting commodities from API: {timer2 - timer1}" +
                                    $"\r\n Saving commodities: {timer3 - timer2}" +
                                    $"\r\n Getting non-commodities from API: {timer4 - timer3}" +
                                    $"\r\n Saving non-commodities: {timer5 - timer4}" +
                                    $"\r\n TOTAL TIME ELAPSED: {timer5 - timer1}");

             await Task.Delay(86_400_000 * 7, stoppingToken);
        }
    }

    public ItemInfoService(ILogger<ItemInfoService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IMapper mapper, IAuctionRequests auctionRequests)
    {
        _logger = logger;
        _configuration = configuration;
        _mapper = mapper;
        // Create Scoped Contexts in Singleton.
        _serviceScopeFactory = serviceScopeFactory;
        _auctionRequests = auctionRequests;

        _region = _configuration["RequestParameters:Region"];
        _realms = _configuration["RequestParameters:RealmId"].Trim().Split(",");
        _locale = _configuration["RequestParameters:Locale"];
    }
}