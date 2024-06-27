using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    /// <summary>
    /// 供奉信息
    /// </summary>
    public class Offering
    {
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [JsonProperty("level")]
        public string? Level { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }
}