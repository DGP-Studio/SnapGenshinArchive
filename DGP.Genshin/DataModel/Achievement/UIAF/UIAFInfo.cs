using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Achievement.UIAF
{
    [SuppressMessage("", "SA1134")]
    [SuppressMessage("", "SA1516")]
    [SuppressMessage("", "SA1600")]
    public class UIAFInfo
    {
        [JsonProperty("export_timestamp")] public long ExportTimeStamp { get; set; }
        [JsonProperty("export_app")] public string? ExportApp { get; set; }
        [JsonProperty("export_app_version")] public string? ExportAppVersion { get; set; }
        [JsonProperty("uiaf_version")] public string? UIGFVersion { get; set; }
    }
}