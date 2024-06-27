using Newtonsoft.Json;

namespace DGP.Genshin.EnkaAPI.Model;

public class Equip
{
    /// <summary>
    /// 物品Id
    /// Equipment ID
    /// </summary>
    [JsonProperty("itemId")]
    public int ItemId { get; set; }

    /// <summary>
    /// 圣遗物
    /// Artifact Base Info
    /// </summary>
    [JsonProperty("reliquary")]
    public Reliquary? Reliquary { get; set; }

    /// <summary>
    /// Detailed Info of Equipment
    /// </summary>
    [JsonProperty("flat")]
    public Flat Flat { get; set; } = default!;

    /// <summary>
    /// 武器
    /// Weapon Base Info
    /// </summary>
    [JsonProperty("weapon")]
    public Weapon? Weapon { get; set; }
}
