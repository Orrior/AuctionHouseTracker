using System.Numerics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApplication1.Utils;

public abstract class WowAuthenticatorRecords
{
    public record OauthTokenResponse
    {
        [JsonPropertyName("access_token")]
        // [DataMember(Name = "access_token")]
        public string AccessToken { get; init; }
        
        [JsonPropertyName("token_type")]
        // [DataMember(Name = "token_type")]
        public string TokenType{ get; init; }
        
        [JsonPropertyName("expires_in")]
        // [DataMember(Name = "expires_in")]
        public int ExpiresIn{ get; init; }
        
        [JsonPropertyName("sub")]
        // [DataMember(Name = "sub")]
        public string Sub{ get; init; }
    }

    public record ItemInfo
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }
        
        [JsonPropertyName("name")]
        public string Name { get; init; }
        
        //ItemClass is workaround since there's no known elegant way to access nested properties.
        
        [JsonPropertyName("item_class")]
        public ItemClass ItemClass { get; init; }

        [JsonPropertyName("item_subclass")]
        public ItemClass ItemSubclass { get; init; }
    }

    public record ItemClass
    {
        [JsonPropertyName("name")]
        public object name { get; init; }
    }


    public record AuctionSlotBase
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }
        
        [JsonPropertyName("item")]
        public Item Item { get; init; }
        
        [JsonPropertyName("quantity")]
        public long Quantity { get; init; } 
        
        [JsonPropertyName("time_left")]
        public string TimeLeft { get; init; }
    }
    
    public record AuctionSlotCommodity : AuctionSlotBase
    {
        [JsonPropertyName("unit_price")]
        // Unit price only in commodities
        public long UnitPrice { get; init; }
    }

    public record AuctionSlotNonCommodity : AuctionSlotBase
    {
        [JsonPropertyName("buyout")]
        // Buyout prices only in non-commodities
        public long BuyOut { get; init; }
        
        public long realmId { get; set; }
        
        [JsonPropertyName("bid")]
        // Buyout prices only in non-commodities
        public long Bid { get; init; }
    }

    public record Item
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }
        
        [JsonPropertyName("bonus_lists")]
        // TODO!! Check if this thing even working
        public List<int> BonusLists { get; init; }
        
        [JsonPropertyName("modifiers")]
        // TODO!! Check if this thing even working
        public List<Dictionary<string, int>> Modifiers { get; init; }
    }
    
    public record AuctionRequestCommodity
    {
        [JsonPropertyName("auctions")]
        public List<AuctionSlotCommodity> AuctionSlots { get; init; }
    }
    
    public record AuctionRequestNonCommodity
    {
        [JsonPropertyName("auctions")]
        public List<AuctionSlotNonCommodity> AuctionSlots { get; init; }
    }

    public record Realms
    {
        [JsonPropertyName("realms")]
        public List<RealmData> realms { get; init; }
    }
    public record RealmData
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }
        
        [JsonPropertyName("id")]
        public long id { get; init; }
    }
}