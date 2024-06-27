using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

public class ReliquaryMainstat
{
    /// <summary>
    /// Equipment Append Property Name.
    /// </summary>
    [JsonProperty("mainPropId")]
    public string MainPropId { get; set; } = default!;

    /// <summary>
    /// Property Value
    /// </summary>
    [JsonProperty("statValue")]
    public double StatValue { get; set; }
}
