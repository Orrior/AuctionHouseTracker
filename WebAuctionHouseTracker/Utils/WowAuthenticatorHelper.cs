using System.Numerics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApplication1.Utils;

public class WowAuthenticatorHelper
{
    public record OauthTokenResponse
    {
        // [JsonPropertyName("access_token")]
        [DataMember(Name = "access_token")]
        public string AccessToken { get; init; }
        
        // [JsonPropertyName("token_type")]
        [DataMember(Name = "token_type")]
        public string TokenType{ get; init; }
        
        // [JsonPropertyName("expires_in")]
        [DataMember(Name = "expires_in")]
        public int ExpiresIn{ get; init; }
        
        // [JsonPropertyName("sub")]
        [DataMember(Name = "sub")]
        public string Sub{ get; init; }
    }

    public record Item
    {
        // [JsonPropertyName("id")]
        [DataMember(Name = "id")]
        public long Id { get; init; }
        
        // [JsonPropertyName("bonus_lists")]
        [DataMember(Name = "bonus_list")]
        public List<int> BonusLists { get; init; }
        
        // [JsonPropertyName("modifiers")]
        [DataMember(Name = "modifiers")]
        public List<Dictionary<string, int>> Modifiers { get; init; }
    }

    public record AuctionSlot
    {
        // [JsonPropertyName("id")]
        [DataMember(Name = "id")]
        public long Id { get; init; }
        
        // [JsonPropertyName("item")]
        [DataMember(Name = "item")]
        public Item Item { get; init; }
        
        // [JsonPropertyName("buyout")]
        [DataMember(Name = "buyout")]
        public long BuyOut { get; init; }
        
        // [JsonPropertyName("quantity")]
        [DataMember(Name = "quantity")]
        public long Quantity { get; init; } 
        
        // [JsonPropertyName("time_left")]
        [DataMember(Name = "time_left")]
        public string TimeLeft { get; init; }
    }
}