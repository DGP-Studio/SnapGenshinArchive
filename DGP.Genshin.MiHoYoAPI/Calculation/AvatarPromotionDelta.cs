using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    /// <summary>
    /// 需要提供的计算信息合集
    /// </summary>
    public class AvatarPromotionDelta
    {
        [JsonProperty("avatar_id")] public int AvatarId { get; set; }
        [JsonProperty("avatar_level_current")] public int AvatarLevelCurrent { get; set; }
        [JsonProperty("avatar_level_target")] public int AvatarLevelTarget { get; set; }
        [JsonProperty("element_attr_id")] public int ElementAttrId { get; set; }
        [JsonProperty("skill_list")] public IEnumerable<PromotionDelta>? SkillList { get; set; }
        [JsonProperty("weapon")] public PromotionDelta? Weapon { get; set; }
        [JsonProperty("reliquary_list")] public IEnumerable<PromotionDelta>? ReliquaryList { get; set; }
    }
}
