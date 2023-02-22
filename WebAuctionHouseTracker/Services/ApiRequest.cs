using RestSharp;
using RestSharp.Authenticators;

namespace WebApplication1.Services;

public class ApiRequest
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> MakeRequest()
    {
        const string region = "eu";
        const string realmId = "3391";
        const string nameSpace = "dynamic-eu";
        const string locale = "en_US";
        
        var accessToken = "EU33onqLlTGYS3b6FdCsXq2CJh9omYHJF9";
        
        var requestUrl = $"https://{region}.api.blizzard.com/data/wow/connected-realm/{realmId}/auctions?namespace={nameSpace}&locale={locale}&access_token=";

        // var content = new FormUrlEncodedContent(values);

        var response = await client.GetAsync("http://www.example.com/recepticle.aspx");

        var responseString = await response.Content.ReadAsStringAsync();
        
        return responseString;
    }
}