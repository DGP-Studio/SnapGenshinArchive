using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.Card
{
    public class DataSwitch
    {
        /// <summary>
        /// 1：个人主页卡片
        /// 2：角色详情数据
        /// 3：实时便笺数据展示
        /// </summary>
        [JsonProperty("switch_id")] public int SwitchId { get; set; }
        [JsonProperty("is_public")] public bool IsPublic { get; set; }
        [JsonProperty("switch_name")] public string? SwitchName { get; set; }
    }
}
