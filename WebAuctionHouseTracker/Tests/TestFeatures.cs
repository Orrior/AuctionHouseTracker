using System.Text.Json;
using Microsoft.AspNetCore.Routing.Matching;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using WebApplication1.Services;
using WebApplication1.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Tests;

public class TestFeatures
{
    public static void Main()
    {
        var _baseUrl = "https://eu.api.blizzard.com";
        var _token = "EU2IAv86W6F7tGZ9aI8xrR47qmSpMZXHxf";
        var _region = "eu";
        var _locale = "en_US";

        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };

        var client = new RestClient(options);

        var request = new RestRequest($"/data/wow/item/{40070}");
        request.AddParameter("namespace", $"static-{_region}");
        request.AddParameter("locale", _locale);
        
        
        var res4 = client.Get<Dictionary<string,Object>>(request).Keys;
        //
        // var res2 = res1.Skip(1).First().Value;
        //
        // var res3 = JsonSerializer.Serialize(res2);
        //
        // var res4 = JsonSerializer.Deserialize<List<Dictionary<string,Object>>>(res3);
        // Console.WriteLine(res4);
    }
}