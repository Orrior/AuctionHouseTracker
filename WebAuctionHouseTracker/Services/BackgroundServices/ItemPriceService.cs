using AutoMapper;
using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Utils;

namespace WebApplication1.Services.BackgroundServices;

public class ItemPriceService : BackgroundService
{
    private readonly ILogger<ItemPriceService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;
    private readonly string[] _realms;
    
    private readonly IAuctionRequests _auctionRequests;
    
    public ItemPriceService(ILogger<ItemPriceService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IMapper mapper, IAuctionRequests auctionRequests)
    {
        _logger = logger;
        _mapper = mapper;
        _serviceScopeFactory = serviceScopeFactory;
        _auctionRequests = auctionRequests;
        
        _realms = configuration["RequestParameters:RealmId"].Trim().Split(",");
    }   
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {JobName}", nameof(ItemPriceService));
        if (string.IsNullOrWhiteSpace(_realms[0]))
        {
            _logger.LogError("Can't get any realms to scan auction Prices! " +
                             "Please fill in connected-realm ids in \"RealmId\", " +
                             "using comma as delimiter in appsettings.json and restart the application");
            return;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var t1 = DateTime.Now;
            
            _logger.LogInformation("Starting price lists update...");
            await using var scope = _serviceScopeFactory.CreateAsyncScope();

            //Create scope and DB
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var timeStamp = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0, 0, DateTimeKind.Utc);

            //Check if scan was made already.
            if (context.CommodityAuctions.Any(x => x.TimeStamp == timeStamp)) 
            {
                _logger.LogError("Scan was made already recently, skipping cycle to the next hour...");
                await Task.Delay(3_600_000, stoppingToken);
                continue; 
            }
        
            ItemPriceServiceHelper.ScanCommodityPrices(context, _auctionRequests, _mapper, timeStamp);
            
            foreach (var realmId in _realms)
            {
                ItemPriceServiceHelper.ScanNonCommodityPrices(context, _auctionRequests, _mapper, timeStamp, realmId);
                _logger.LogInformation("Connected realm with id {RealmId} was scanned successfully!", realmId);
            }

            _logger.LogInformation("===TOTAL SAVED===\r\n Prices saved in {Timer}! Next scan will be in 1 hour", 
                DateTime.Now - t1);
            
            await Task.Delay(3_600_000, stoppingToken);
        }
    }
}