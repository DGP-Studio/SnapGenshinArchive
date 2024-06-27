using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.EnkaAPI.Model;

public class Reliquary
{
    /// <summary>
    /// 等级 +20 = 21
    /// [1,21]
    /// Artifact Level [1-21]
    /// </summary>
    [JsonProperty("level")]
    public int Level { get; set; }

    /// <summary>
    /// 主属性Id
    /// Artifact Main Stat ID
    /// </summary>
    [JsonProperty("mainPropId")]
    public int MainPropId { get; set; }

    /// <summary>
    /// 强化属性Id
    /// </summary>
    [JsonProperty("appendPropIdList")]
    public IList<int> AppendPropIdList { get; set; } = default!;
}
