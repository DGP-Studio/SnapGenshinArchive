using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Weapon
{
    /// <summary>
    /// 角色统计值
    /// </summary>
    [SuppressMessage("", "SA1134")]
    [SuppressMessage("", "SA1516")]
    [SuppressMessage("", "SA1600")]
    public class WeaponStatValues
    {
        [JsonProperty("1")] public string? Level1 { get; set; }
        [JsonProperty("5")] public string? Level5 { get; set; }
        [JsonProperty("10")] public string? Level10 { get; set; }
        [JsonProperty("15")] public string? Level15 { get; set; }
        [JsonProperty("20")] public string? Level20 { get; set; }
        [JsonProperty("20+")] public string? Level20p { get; set; }
        [JsonProperty("25")] public string? Level25 { get; set; }
        [JsonProperty("30")] public string? Level30 { get; set; }
        [JsonProperty("35")] public string? Level35 { get; set; }
        [JsonProperty("40")] public string? Level40 { get; set; }
        [JsonProperty("40+")] public string? Level40p { get; set; }
        [JsonProperty("45")] public string? Level45 { get; set; }
        [JsonProperty("50")] public string? Level50 { get; set; }
        [JsonProperty("50+")] public string? Level50p { get; set; }
        [JsonProperty("55")] public string? Level55 { get; set; }
        [JsonProperty("60")] public string? Level60 { get; set; }
        [JsonProperty("60+")] public string? Level60p { get; set; }
        [JsonProperty("65")] public string? Level65 { get; set; }
        [JsonProperty("70")] public string? Level70 { get; set; }
        [JsonProperty("70+")] public string? Level70p { get; set; }
        [JsonProperty("75")] public string? Level75 { get; set; }
        [JsonProperty("80")] public string? Level80 { get; set; }
        [JsonProperty("80+")] public string? Level80p { get; set; }
        [JsonProperty("85")] public string? Level85 { get; set; }
        [JsonProperty("90")] public string? Level90 { get; set; }
    }
}