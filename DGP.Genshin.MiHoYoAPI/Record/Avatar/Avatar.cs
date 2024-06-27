using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.Avatar
{
    /// <summary>
    /// 包含一个角色的基础信息
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        [JsonProperty("image")]
        public string? Image { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 元素英文名称
        /// </summary>
        [JsonProperty("element")]
        public string? Element { get; set; }

        /// <summary>
        /// 好感度
        /// </summary>
        [JsonProperty("fetter")]
        public int Fetter { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// 稀有度
        /// </summary>
        [JsonProperty("rarity")]
        public int Rarity { get; set; }

        /// <summary>
        /// 激活的命座数
        /// </summary>
        [JsonProperty("actived_constellation_num")]
        public int ActivedConstellationNum { get; set; }
    }
}