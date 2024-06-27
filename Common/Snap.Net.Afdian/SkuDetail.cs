using Newtonsoft.Json;

namespace Snap.Net.Afdian
{
    public class SkuDetail
    {
        [JsonProperty("sku_id")] public string? SkuId { get; set; }
        [JsonProperty("count")] public long Count { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("album_id")] public string? AlbumId { get; set; }
        [JsonProperty("pic")] public string? Pic { get; set; }
    }
}
