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
        var _token = "EUHpVWyZmcwrb7EaCTRqnomPG9kTXIowKY";
        var _region = "eu";
        var _locale = "en_US";


            var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };
        var client = new RestClient(options);
        var request = new RestRequest($"/data/wow/realm/index");
        request.AddParameter("namespace", $"dynamic-{_region}");
        request.AddParameter("locale", _locale);

        var result = client.Get<WowAuthenticatorRecords.Realms>(request).realms;

        Console.WriteLine(String.Join(",",result.Select(x => $"Id:{x.id} Name:{x.Name}").ToList()));

        
    }
}