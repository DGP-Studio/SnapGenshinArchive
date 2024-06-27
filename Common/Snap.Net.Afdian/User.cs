using Newtonsoft.Json;

namespace Snap.Net.Afdian
{
    public class User
    {
        [JsonProperty("user_id")] public string? UserId { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("avatar")] public string? Avatar { get; set; }
    }
}
