using AutoMapper;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Services.BackgroundServices;

public class ItemPriceRequestService : BackgroundService
{
    private readonly ILogger<ItemPriceRequestService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    private string _region;
    private string[] _realms;
    private string _locale;
    
    private string token = "";
    private readonly IAuctionRequests _auctionRequests;
    
    public ItemPriceRequestService(ILogger<ItemPriceRequestService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IMapper mapper, IAuctionRequests auctionRequests)
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
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {jobName}", nameof(ItemPriceRequestService));
        var timer1 = DateTime.Now;
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Starting price lists update...");
            
            //Create scope and DB
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            //TODO UNCOMMENT!!!
            
            //scan commodities
            var commodities = _auctionRequests.GetCheapestCommodities().Result
                .Select(x => _mapper.Map<CommodityAuction>(x)).ToList();
            
            _logger.LogDebug("Commodities prices are saved!");
            
            //save commodities
            await context.CommodityAuctions.AddRangeAsync(commodities);
            await context.SaveChangesAsync();
            
            //start for-loop for each region to be scanned and saved.
            foreach (var realmId in _realms)
            {
                _logger.LogInformation($"Scanning non-commodities for realm with id {realmId}...");
                
                var nonCommodities = _auctionRequests.GetCheapestNonCommodities(realmId).Result
                    .Select(x => _mapper.Map<NonCommodityAuction>(x)).ToList();
            
                await context.NonCommodityAuctions.AddRangeAsync(nonCommodities);
                await context.SaveChangesAsync();
                
                _logger.LogDebug($"Non-commodities prices for id '{realmId}' are saved!");
            }
            
            var timer2 = DateTime.Now;
            
            _logger.LogInformation($"All prices are saved in {timer2-timer1}! Next scan will be in 1 hour.");
            await Task.Delay(3_600_000, stoppingToken);
        }
    }
}