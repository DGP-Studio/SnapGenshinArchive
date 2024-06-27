using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Calculation.Filter
{
    public class ReliquaryIdFilter
    {
        /// <summary>
        /// 稀有度
        /// </summary>
        [JsonProperty("reliquary_levels")] public List<int>? WeaponLevels { get; set; } = new();
        /// <summary>
        /// 1 代表 花
        /// </summary>
        [JsonProperty("reliquary_cat_id")] public int ReliquaryCatId { get; set; } = 1;
        [JsonProperty("page")] public int Page { get; set; }
        /// <summary>
        /// 请求页的尺寸，默认20
        /// </summary>
        [JsonProperty("size")] public int Size { get; set; } = 20;
    }
}
