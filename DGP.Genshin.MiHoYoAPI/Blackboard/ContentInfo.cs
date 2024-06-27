using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Blackboard
{
    public class ContentInfo
    {
        [JsonProperty("content_id")] public int ContentId { get; set; }
        [JsonProperty("title")] public string? Title { get; set; }
        [JsonProperty("icon")] public string? Icon { get; set; }
        [JsonProperty("bbs_url")] public string? BbsUrl { get; set; }
    }
}
