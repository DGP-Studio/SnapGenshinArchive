using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Weapon
{
    [SuppressMessage("", "SA1134")]
    [SuppressMessage("", "SA1516")]
    [SuppressMessage("", "SA1600")]
    public class PassiveDescription
    {
        [JsonProperty("Lv1")] public string? Level1 { get; set; }
        [JsonProperty("Lv2")] public string? Level2 { get; set; }
        [JsonProperty("Lv3")] public string? Level3 { get; set; }
        [JsonProperty("Lv4")] public string? Level4 { get; set; }
        [JsonProperty("Lv5")] public string? Level5 { get; set; }
    }
}