using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    public class UIGFInfo
    {
        [JsonProperty("uid")] public string? Uid { get; set; }
        [JsonProperty("lang")] public string? Language { get; set; }
        [JsonProperty("export_time")] public string? ExportTime { get; set; }
        [JsonProperty("export_timestamp")] public long ExportTimeStamp { get; set; }
        [JsonProperty("export_app")] public string? ExportApp { get; set; }
        [JsonProperty("export_app_version")] public string? ExportAppVersion { get; set; }
        [JsonProperty("uigf_version")] public string? UIGFVersion { get; set; }
    }
}
