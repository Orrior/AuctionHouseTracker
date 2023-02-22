using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;

namespace WebApplication1.Utils;

public class WoWAuthenticator
{
    private const string ClientId = "";
    private const string ClientSecret = "";

    private const string region = "eu";
    private const string realmId = "3391";
    private const string nameSpace = "dynamic-eu";
    private const string locale = "en_US";

    private const string baseUrl = $"https://{region}.api.blizzard.com";

    public static async Task<string> GetToken()
    {
        var options = new RestClientOptions("https://oauth.battle.net")
        { Authenticator = new HttpBasicAuthenticator(ClientId,ClientSecret) };
        
        var client = new RestClient(options);
        
        var request = new RestRequest("/token")
            .AddParameter("grant_type", "client_credentials");
        
        var response = await client.PostAsync<WowAuthenticatorHelper.OauthTokenResponse>(request);
        // var response = await client.PostAsync<Dictionary<string, object>>(request);
        
        return response.AccessToken;
    }
    
    public static async Task<bool> CheckToken(string token)
    {
        //Returns true if token is still lasting.
        
        var client = new RestClient("https://oauth.battle.net");
        var request = new RestRequest("/oauth/check_token").AddParameter("token", token);
        var response = await client.GetAsync<Dictionary<string, object>>(request);
        
        var tokenTime = DateTime.UnixEpoch.AddSeconds(int.Parse(response?["exp"].ToString() ?? string.Empty));

        return DateTime.UtcNow < tokenTime;
    }

    //TODO!!! V Make second argument "realm" later, also make this faster.
    public static async Task<List<WowAuthenticatorHelper.AuctionSlot>> GetNonCommodities(string token)
    {
        //NB!!! Result of this will be unique for each realm! Only commodities are cross-realm!

        var options = new RestClientOptions(baseUrl)
        { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer") };
        var client = new RestClient(options);
        var request = new RestRequest($"/data/wow/connected-realm/{realmId}/auctions?namespace={nameSpace}&locale={locale}");
        
        var response = await client.GetAsync<Dictionary<string, object>>(request);
        
        //TODO!!! THIS IS VERY MEMORY-INTENSIVE! Implement JsonTextReader later! https://stackoverflow.com/questions/32227436/parsing-large-json-file-in-net
        var auctionSlotsString = response?["auctions"].ToString();

        if (auctionSlotsString == null) return new List<WowAuthenticatorHelper.AuctionSlot>();
        var auctionSlots = JsonSerializer.Deserialize<List<WowAuthenticatorHelper.AuctionSlot>>(auctionSlotsString);
        return auctionSlots!;
    }
    
    public static async Task<List<WowAuthenticatorHelper.AuctionSlot>> GetCommodities(string token)
    {
        var options = new RestClientOptions(baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer") };
        var client = new RestClient(options);
        
        var request = new RestRequest($"/data/wow/auctions/commodities");
        request.AddParameter("namespace", nameSpace);
        request.AddParameter("locale", locale);
        
        var response = await client.GetAsync<Dictionary<string, object>>(request);
        
        var auctionSlotsString = response?["auctions"].ToString();

        if (auctionSlotsString == null) return new List<WowAuthenticatorHelper.AuctionSlot>();
        var auctionSlots = JsonSerializer.Deserialize<List<WowAuthenticatorHelper.AuctionSlot>>(auctionSlotsString);
        return auctionSlots!;
    }
}