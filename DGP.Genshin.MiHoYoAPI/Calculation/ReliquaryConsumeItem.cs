using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    public class ReliquaryConsumeItem
    {
        [JsonProperty("reliquary_id")] public int ReliquaryId { get; set; }
        /// <summary>
        /// 圣遗物品质
        /// </summary>
        [JsonProperty("id_consume_list")] public List<ConsumeItem>? IdConsumeList { get; set; }
    }
}
