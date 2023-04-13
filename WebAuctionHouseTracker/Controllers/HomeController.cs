using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Web;
using WebApplication1.Services.Auction;
using WebApplication1.Utils;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly List<WowAuthenticatorRecords.RealmData> _allRealms;
    private readonly List<string> _realms;

    public HomeController(ILogger<HomeController> logger, IAuctionRequests auctionRequests, IConfiguration configuration)
    {
        _logger = logger;
        _allRealms = auctionRequests.GetRealmNames();
        _realms = SettingsHelper.parseRealms(configuration["RequestParameters:RealmId"]) ;
    }

    public IActionResult Index()
    {
        Dictionary<string, string> realms = new Dictionary<string, string>();
        bool realmsAreSet;
        
        if (_realms.Count > 0)
        {
            realmsAreSet = true;
            foreach (var realm in _realms)
            {
                var realmItem = _allRealms.Find(x => x.id.ToString() == realm);

                if (realmItem != null)
                {
                    realms.Add(realm, realmItem.Name);
                }
            }
        }
        else
        {
            realmsAreSet = false;
            foreach (var realm in _allRealms)
            {
                realms.Add(realm.id.ToString(),realm.Name);
            }
        }

        
        
        Console.WriteLine($"_Realms count: {_realms.Count}");
        Console.WriteLine(JsonSerializer.Serialize(_realms));
        Console.WriteLine($"RealmsAreSet: {realmsAreSet.ToString()}");


        return View(new RealmsModel{Realms = realms, RealmsAreSet = realmsAreSet});
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
