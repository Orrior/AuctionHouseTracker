using System.Collections;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Utils;

public static class WoWAuthenticator
{
    
    public static async Task<string> GetToken(string clientId, string clientSecret)
    {
        var options = new RestClientOptions("https://oauth.battle.net")
        { Authenticator = new HttpBasicAuthenticator(clientId,clientSecret) };
        
        var client = new RestClient(options);

        var request = new RestRequest("/token")
            .AddParameter("grant_type", "client_credentials")
            .AddParameter("scope", "wow.profile");

        var response = await client.PostAsync<WowAuthenticatorRecords.OauthTokenResponse>(request);

        return response.AccessToken;
    }
    
    public static async Task<bool> CheckToken(string token)
    {
        //Returns true if token is still active.
        try
        {
            var client = new RestClient("https://oauth.battle.net");
            var request = new RestRequest("/oauth/check_token").AddParameter("token", token);
            var response = await client.GetAsync<Dictionary<string, object>>(request);

            var tokenTime = DateTime.UnixEpoch.AddSeconds(int.Parse(response?["exp"].ToString() ?? string.Empty));

            return DateTime.UtcNow < tokenTime;
        }
        catch (Exception e)
        {
            return false;
        }

    }

    public static async Task<string> RefreshToken<T>(string token, string clientdId, string clientSecret, ILogger<T> logger)
    {
        if (!await WoWAuthenticator.CheckToken(token))
        {
            token = await WoWAuthenticator.GetToken(clientdId, clientSecret);
                
            logger.LogInformation("Old token has expired. New token: \"{}\".",token);
        }
        else
        {
            logger.LogInformation("Token \"{}\" is still valid.", token);
        }
        return token;
    }
}