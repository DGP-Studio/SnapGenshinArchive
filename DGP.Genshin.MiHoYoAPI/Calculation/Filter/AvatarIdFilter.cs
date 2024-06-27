using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Calculation.Filter
{
    public class AvatarIdFilter
    {
        [JsonProperty("element_attr_ids")] public List<int>? ElementAttrIds { get; set; } = new();
        [JsonProperty("weapon_cat_ids")] public List<int>? WeaponCatIds { get; set; } = new();
        [JsonProperty("page")] public int Page { get; set; }
        /// <summary>
        /// 请求页的尺寸，默认20
        /// </summary>
        [JsonProperty("size")] public int Size { get; set; } = 20;
    }
}
