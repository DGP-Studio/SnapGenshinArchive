using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.EnkaAPI.Model;

public class Flat
{
    /// <summary>
    /// 名称
    /// Hash for Equipment Name
    /// </summary>
    [JsonProperty("nameTextMapHash")]
    public string NameTextMapHash { get; set; } = default!;

    /// <summary>
    /// 套装名称
    /// Hash for Artifact Set Name
    /// </summary>
    [JsonProperty("setNameTextMapHash")]
    public string? SetNameTextMapHash { get; set; }

    /// <summary>
    /// 等级
    /// Rarity Level of Equipment
    /// </summary>
    [JsonProperty("rankLevel")]
    public int RankLevel { get; set; }

    /// <summary>
    /// 圣遗物主属性
    /// Artifact Main Stat
    /// </summary>
    [JsonProperty("reliquaryMainstat")]
    public ReliquaryMainstat? ReliquaryMainstat { get; set; }

    /// <summary>
    /// 圣遗物副属性
    /// List of Artifact Substats
    /// </summary>
    [JsonProperty("reliquarySubstats")]
    public IList<ReliquarySubstat>? ReliquarySubstats { get; set; }

    /// <summary>
    /// 物品类型
    /// Equipment Type: Weapon or Artifact
    /// ITEM_WEAPON
    /// ITEM_RELIQUARY
    /// </summary>
    [JsonProperty("itemType")]
    public string ItemType { get; set; } = default!;

    /// <summary>
    /// 图标
    /// Equipment Icon Name
    /// </summary>
    [JsonProperty("icon")]
    public string Icon { get; set; } = default!;

    /// <summary>
    /// 圣遗物类型
    /// Artifact Type
    /// EQUIP_BRACER   Flower
    /// EQUIP_NECKLACE Feather
    /// EQUIP_SHOES    Sands
    /// EQUIP_RING	   Goblet
    /// EQUIP_DRESS    Circlet
    /// </summary>
    [JsonProperty("equipType")]
    public string? EquipType { get; set; }

    /// <summary>
    /// 武器主副属性
    /// 0 基础攻击力
    /// 1 主属性
    /// List of Weapon Stat: Base ATK, Substat
    /// </summary>
    [JsonProperty("weaponStats")]
    public IList<WeaponStat>? WeaponStats { get; set; }
}
