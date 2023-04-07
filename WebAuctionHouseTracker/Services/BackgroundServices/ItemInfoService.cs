using System.Text.Json;
using AutoMapper;
using WebApplication1.Migrations;
using WebApplication1.Models;
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
            _logger.LogInformation("Starting info lists update... \r\n" +
                                   $"Region:{_region},Realms:[{string.Join(',', _realms)}],Locale:{_locale}");

            // Check Whether token is valid, if not - get new token.
            token = await WoWAuthenticator.RefreshToken(
                token, 
                _configuration["OAuth2Credentials:ClientId"],
                _configuration["OAuth2Credentials:ClientSecret"],
                _logger);
            
            //Create scope and DB
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //Scan all commodity items and add them to DB
            _logger.LogInformation("Start scanning commodities:");

            var comInfosMapped = await _auctionRequests.GetCommodityInfos();
             
            _logger.LogInformation($"Commodities scanning has finished! Scanned {comInfosMapped.Count} items!");
            
             
             //TODO!!!!
             //add cominfoMapped items to DB.
             //Save db changes.
             
             
             
             _logger.LogInformation("Start scanning non-commodities");

             // //Scan all noncommodity items and add them to DB.
            // var nonComItems = await auction.GetCheapestNonCommodities(_realms[0]);
            // var nonComInfos = auction.GetItemInfos(new List<AuctionSlotBase>(nonComItems));
            
            
            
            await Task.Delay(3_600_000 * 7, stoppingToken);
        }
        


        //1.Получить уникальные коммодити и некоммодити
        //2.Делать по этим предметам реквесты по 10шт за секунду, каждые 10 пачек сохранять на бд
        //3.Мапнуть инфопредметы в CommodityInfo и NonCommodityInfo, добавить их в базу данных.
        
        throw new NotImplementedException();
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