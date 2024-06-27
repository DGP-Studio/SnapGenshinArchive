using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    public class Avatar : Calculable
    {
        /// <summary>
        /// 角色的星级
        /// </summary>
        [JsonProperty("avatar_level")] public int AvatarLevel { get; set; }
        [JsonProperty("weapon_cat_id")] public int WeaponCatId { get; set; }
        [JsonProperty("element_attr_id")] public int ElementAttrId { get; set; }
    }
}
