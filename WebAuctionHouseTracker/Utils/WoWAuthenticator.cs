using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using RestSharp;
using RestSharp.Authenticators;

namespace WebApplication1.Utils;

public class WoWAuthenticator
{
    private const string ClientId = "";
    private const string ClientSecret = "";

    //Use record for mapping response in GetToken().
    
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
}