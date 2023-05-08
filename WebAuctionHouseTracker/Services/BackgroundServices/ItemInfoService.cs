using WebApplication1.Interfaces;
using WebApplication1.Migrations;
using WebApplication1.Utils;

namespace WebApplication1.Services.BackgroundServices;

public class ItemInfoService : BackgroundService
{
    private readonly ILogger<ItemInfoService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly string _region;
    private readonly string[] _realms;
    private readonly string _locale;
    private readonly IAuctionRequests _auctionRequests;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {JobName}", nameof(ItemPriceService));

        if (string.IsNullOrWhiteSpace(_realms[0]))
        {
            _logger.LogError("Can't get any realms to scan item infos! " +
                             "Please fill in connected-realm ids in \"RealmId\", " +
                             "using comma as delimiter in appsettings.json and restart the application");
            return;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var t1 = DateTime.Now;
            _logger.LogInformation("Starting info lists update... \r\n" + 
                                   "Region:{Region},Realms:[{Realms}],Locale:{Locale}",
                _region, string.Join(',', _realms), _locale);

            //Create scope and DB
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            _logger.LogInformation("Start scanning commodities info...");
            ItemInfoServiceHelper.ScanCommodityInfo(context, _auctionRequests);
            
            _logger.LogInformation("Start scanning non-commodities...");
            ItemInfoServiceHelper.ScanNonCommodityInfo(context, _auctionRequests, _realms[0]);

            _logger.LogInformation("Scan completed... \r\n TOTAL TIME ELAPSED: {Now}", DateTime.Now- t1);

            await Task.Delay(86_400_000 * 7, stoppingToken);
        }
    }

    public ItemInfoService(ILogger<ItemInfoService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IAuctionRequests auctionRequests)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _auctionRequests = auctionRequests;

        _region = configuration["RequestParameters:Region"];
        _realms = configuration["RequestParameters:RealmId"].Trim().Split(",");
        _locale = configuration["RequestParameters:Locale"];
    }
}