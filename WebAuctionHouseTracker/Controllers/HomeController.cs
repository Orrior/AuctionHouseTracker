using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models.Web;
using WebApplication1.Utils;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly List<string> _realms;
    private readonly IAuctionRequests _auctionRequests;

    public HomeController(IAuctionRequests auctionRequests, IConfiguration configuration)
    {
        _realms = SettingsHelper.parseRealms(configuration["RequestParameters:RealmId"]);
        _auctionRequests = auctionRequests;
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
                var connectedRealms = _auctionRequests.GetConnectedRealm(realm);
                
                if (connectedRealms.Count != 0)
                {
                    realms.Add(realm, String.Join(", ", connectedRealms.Select(x => x.Name).ToList()));
                }
            }
        }
        else
        {
            realmsAreSet = false;
            foreach (var realm in _auctionRequests.GetConnectedRealms())
            {
                realms.Add(realm.Key ,String.Join(", ",realm.Value.Select(x => x.Name).ToList()));
            }
        }
        
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
