using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

public class WeaponStat
{
    [JsonProperty("appendPropId")]
    public string AppendPropId { get; set; } = default!;

    [JsonProperty("statValue")]
    public double StatValue { get; set; }
}
