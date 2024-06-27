using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Calculation.Filter
{
    public class WeaponIdFilter
    {
        /// <summary>
        /// 武器稀有度
        /// </summary>
        [JsonProperty("weapon_levels")] public List<int>? WeaponLevels { get; set; } = new();
        [JsonProperty("weapon_cat_ids")] public List<int>? WeaponCatIds { get; set; }
        [JsonProperty("page")] public int Page { get; set; }
        /// <summary>
        /// 请求页的尺寸，默认20
        /// </summary>
        [JsonProperty("size")] public int Size { get; set; } = 20;
    }
}
