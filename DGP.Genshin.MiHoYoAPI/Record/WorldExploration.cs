using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    /// <summary>
    /// 世界探索
    /// </summary>
    public class WorldExploration
    {
        /// <summary>
        /// 等级
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// 探索度
        /// Maxmium is 1000
        /// </summary>
        [JsonProperty("exploration_percentage")]
        public int ExplorationPercentage { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 类型 Offering Reputation
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }

        /// <summary>
        /// 供奉进度
        /// </summary>
        [JsonProperty("offerings")]
        public List<Offering>? Offerings { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 父ID 当无链接的父对象时为0
        /// </summary>
        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 地图链接
        /// </summary>
        [JsonProperty("map_url")]
        public string? MapUrl { get; set; }

        /// <summary>
        /// 攻略链接 无攻略时为 <see cref="string.Empty"/>
        /// </summary>
        [JsonProperty("strategy_url")]
        public string? StrategyUrl { get; set; }

        /// <summary>
        /// 背景图片
        /// </summary>
        [JsonProperty("background_image")]
        public string? BackgroundImage { get; set; }

        /// <summary>
        /// 反色图标
        /// </summary>
        [JsonProperty("inner_icon")]
        public string? InnerIcon { get; set; }

        /// <summary>
        /// 背景图片
        /// </summary>
        [JsonProperty("cover")]
        public string? Cover { get; set; }

        /// <summary>
        /// 百分比*100进度
        /// </summary>
        public double ExplorationPercentageBy10
        {
            get => ExplorationPercentage / 10.0;
        }

        /// <summary>
        /// 指示当前是否为声望
        /// </summary>
        public bool IsReputation
        {
            get => Type == "Reputation";
        }

        /// <summary>
        /// 类型名称转换器
        /// </summary>
        public string ConvertedType
        {
            get => IsReputation ? "声望等级" : "供奉等级";
        }
    }
}