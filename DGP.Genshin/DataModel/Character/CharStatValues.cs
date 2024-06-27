using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Character
{
    /// <summary>
    /// 角色统计值
    /// </summary>
    [SuppressMessage("", "SA1134")]
    [SuppressMessage("", "SA1516")]
    [SuppressMessage("", "SA1600")]
    public class CharStatValues
    {
        [JsonProperty("1")] public string? Level1 { get; set; }
        [JsonProperty("20")] public string? Level20 { get; set; }
        [JsonProperty("20+")] public string? Level20p { get; set; }
        [JsonProperty("40")] public string? Level40 { get; set; }
        [JsonProperty("40+")] public string? Level40p { get; set; }
        [JsonProperty("50")] public string? Level50 { get; set; }
        [JsonProperty("50+")] public string? Level50p { get; set; }
        [JsonProperty("60")] public string? Level60 { get; set; }
        [JsonProperty("60+")] public string? Level60p { get; set; }
        [JsonProperty("70")] public string? Level70 { get; set; }
        [JsonProperty("70+")] public string? Level70p { get; set; }
        [JsonProperty("80")] public string? Level80 { get; set; }
        [JsonProperty("80+")] public string? Level80p { get; set; }
        [JsonProperty("90")] public string? Level90 { get; set; }
    }
}