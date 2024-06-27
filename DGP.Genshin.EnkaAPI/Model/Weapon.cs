using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.EnkaAPI.Model;

public class Weapon
{
    /// <summary>
    /// 等级
    /// Weapon Level
    /// </summary>
    [JsonProperty("level")]
    public int Level { get; set; }

    /// <summary>
    /// 突破等级
    /// Weapon Ascension Level
    /// </summary>
    [JsonProperty("promoteLevel")]
    public int PromoteLevel { get; set; }

    /// <summary>
    /// 精炼 相较于实际等级 -1
    /// Weapon Refinement Level [0-4]
    /// </summary>
    [JsonProperty("affixMap")]
    public IDictionary<string, int> AffixMap { get; set; } = default!;
}
