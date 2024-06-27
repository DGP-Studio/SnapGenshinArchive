using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.EnkaAPI.Model;

public class AvatarInfo
{
    /// <summary>
    /// 角色Id
    /// Character ID
    /// </summary>
    [JsonProperty("avatarId")]
    public int AvatarId { get; set; }

    /// <summary>
    /// 基础属性
    /// Character Info Properties List
    /// </summary>
    [JsonProperty("propMap")]
    public IDictionary<string, TypeValue> PropMap { get; set; } = default!;

    /// <summary>
    /// 属性Map
    /// Map of Character's Combat Properties.
    /// </summary>
    [JsonProperty("fightPropMap")]
    public IDictionary<string, double> FightPropMap { get; set; } = default!;

    /// <summary>
    /// 技能组Id
    /// Character Skill Set ID
    /// </summary>
    [JsonProperty("skillDepotId")]
    public int SkillDepotId { get; set; }

    /// <summary>
    /// List of Unlocked Skill Ids
    /// 被动天赋
    /// </summary>
    [JsonProperty("inherentProudSkillList")]
    public IList<int> InherentProudSkillList { get; set; } = default!;

    /// <summary>
    /// Map of Skill Levels
    /// </summary>
    [JsonProperty("skillLevelMap")]
    public IDictionary<string, int> SkillLevelMap { get; set; } = default!;

    /// <summary>
    /// 装备列表
    /// 最后一个为武器
    /// List of Equipments: Weapon, Ariftacts
    /// </summary>
    [JsonProperty("equipList")]
    public IList<Equip> EquipList { get; set; } = default!;

    /// <summary>
    /// 好感度信息
    /// Character Friendship Level
    /// </summary>
    [JsonProperty("fetterInfo")]
    public FetterInfo FetterInfo { get; set; } = default!;

    /// <summary>
    /// 命座 Id
    /// </summary>
    [JsonProperty("talentIdList")]
    public IList<int> TalentIdList { get; set; } = default!;

    /// <summary>
    /// 皮肤 Id
    /// </summary>
    [JsonProperty("costumeId")]
    public int? CostumeId { get; set; }

    /// <summary>
    /// 命座额外技能等级
    /// </summary>
    [JsonProperty("proudSkillExtraLevelMap")]
    public ProudSkillExtraLevelMap ProudSkillExtraLevelMap { get; set; } = default!;
}
