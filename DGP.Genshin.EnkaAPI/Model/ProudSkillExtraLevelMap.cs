using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

public class ProudSkillExtraLevelMap
{
    [JsonProperty("3932")]
    public int A3932 { get; set; }

    [JsonProperty("3939")]
    public int A3939 { get; set; }

    [JsonProperty("2532")]
    public int? A2532 { get; set; }

    [JsonProperty("2539")]
    public int? A2539 { get; set; }

    [JsonProperty("3231")]
    public int? A3231 { get; set; }

    [JsonProperty("3232")]
    public int? A3232 { get; set; }

    [JsonProperty("3239")]
    public int? A3239 { get; set; }
}
