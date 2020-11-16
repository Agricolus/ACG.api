using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ACG.api.Dtos
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class KeyrockTokenResponse
    {
        // [JsonProperty("access_token")]
        // [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string AccessToken { get; set; }
        // [JsonProperty("token_type")]
        // [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string TokenType { get; set; }
        // [JsonProperty("expires_in")]
        // [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public int ExpiresIn { get; set; }
        // [JsonProperty("refresh_token")]
        // [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public string RefreshToken { get; set; }
    }
}
