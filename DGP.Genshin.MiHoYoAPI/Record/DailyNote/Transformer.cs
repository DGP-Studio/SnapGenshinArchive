using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.DailyNote
{
    /// <summary>
    /// 参量质变仪
    /// </summary>
    public class Transformer
    {
        /// <summary>
        /// 是否拥有该道具
        /// </summary>
        [JsonProperty("obtained")]
        public bool Obtained { get; set; }

        /// <summary>
        /// 恢复时间包装
        /// </summary>
        [JsonProperty("recovery_time")]
        public RecoveryTime? RecoveryTime { get; set; }

        /// <summary>
        /// Wiki链接
        /// </summary>
        [JsonProperty("wiki")]
        public string? Wiki { get; set; }
    }
}