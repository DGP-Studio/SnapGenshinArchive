using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    /// <summary>
    /// 计算信息
    /// </summary>
    public class PromotionDelta
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("level_current")] public int LevelCurrent { get; set; }
        [JsonProperty("level_target")] public int LevelTarget { get; set; }
    }
}
