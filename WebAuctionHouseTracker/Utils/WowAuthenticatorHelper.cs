using System.Text.Json.Serialization;

namespace WebApplication1.Utils;

public class WowAuthenticatorHelper
{
    public record OauthTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
        
        [JsonPropertyName("token_type")]
        public string TokenType{ get; init; }
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn{ get; init; }
        
        [JsonPropertyName("sub")]
        public string Sub{ get; init; }
    }
}