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
    
    private string token = "";
    public ItemPriceRequestService(ILogger<ItemPriceRequestService> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        // Create Scoped Contexts in Singleton.
        _serviceScopeFactory = serviceScopeFactory;
    }   
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {jobName}", nameof(ItemPriceRequestService));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Starting price lists update...");
            
            // Check Whether token is valid, if not - get new token.
            if (!await WoWAuthenticator.CheckToken(token))
            {
                //TODO!!! Make check for configuration fields during start in Program.cs
                token = await WoWAuthenticator.GetToken(
                    _configuration["OAuth2Credentials:ClientId"], 
                    _configuration["OAuth2Credentials:ClientSecret"]);
                
                _logger.LogInformation("Old token has expired. New token: \"{}\".",token);
            }
            else
            {
                _logger.LogInformation("Token \"{}\" is still valid.", token);
            }

            //Create Database context
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

           //  Console.WriteLine(context.Database.ProviderName);
           //
           //
           //  var testInfo = new CommodityInfo()
           //  {
           //      Id = 18635,
           //      Name = "TEsT ITEM",
           //      ItemClass = "TESTCLASS",
           //      ItemSubClass = "TESTSUBCLASS"
           //  };
           //  
           //  var testAuction = new CommodityAuction {
           //      CommodityId = 18635, 
           //      Quantity = 19, 
           //      UnitPrice = 98890000, 
           //      TimeLeft = "VERY_LONG", 
           //      TimeStamp = DateTime.Now};
           //
           //  Console.WriteLine("TRY TO ADD OBJECT TO DATABASE");
           //  
           //  context.CommodityAuctions.Add(testAuction);
           //  await context.SaveChangesAsync(stoppingToken);
           //  
           //  Console.WriteLine("ADD COMPLETE");
           //  Console.WriteLine("RESULT:");
           //  Console.WriteLine(context.CommodityAuctions);
           //  Console.WriteLine(JsonSerializer.Serialize(context.CommodityAuctions));
           // ;
            
            // Create AuctionRequest, Get all Realms and region
            // var auction = new AuctionRequests(
            //     _configuration["RequestParameters:Region"], 
            //     _configuration["RequestParameters:Locale"], token);

            var realms = _configuration["RequestParameters:RealmId"];

            // Request Commodity, update db price list.
            
            // Request all Non-Commodity, update db price list.
            
            // Wait one hour (3_600_000 milliseconds).
            await Task.Delay(3_600_000, stoppingToken);
        }
    }
}