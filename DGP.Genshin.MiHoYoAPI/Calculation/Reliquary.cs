using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    public class Reliquary : Calculable
    {
        [JsonProperty("reliquary_cat_id")] public int ReliquaryCatId { get; set; }
        /// <summary>
        /// 圣遗物品质
        /// </summary>
        [JsonProperty("reliquary_level")] public int ReliquaryLevel { get; set; }
    }
}
