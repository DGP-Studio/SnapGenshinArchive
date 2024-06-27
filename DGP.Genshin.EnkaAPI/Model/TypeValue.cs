using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

public class TypeValue
{
    /// <summary>
    /// 类型Id
    /// </summary>
    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("val")]
    public string? Value { get; set; }
}
