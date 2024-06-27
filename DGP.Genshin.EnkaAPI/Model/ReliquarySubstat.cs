using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

public class ReliquarySubstat
{
    /// <summary>
    /// 增加属性
    /// </summary>
    [JsonProperty("appendPropId")]
    public string AppendPropId { get; set; } = default!;

    /// <summary>
    /// 值
    /// </summary>
    [JsonProperty("statValue")]
    public double StatValue { get; set; }
}
