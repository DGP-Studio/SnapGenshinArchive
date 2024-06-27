using Newtonsoft.Json;
using Snap.Data.Primitive;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    /// <summary>
    /// 可计算的物品
    /// </summary>
    public class Calculable : Observable
    {
        private int levelTarget;

        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// 默认值设为1，因为部分API不返回该字段
        /// </summary>
        [JsonProperty("level_current")]
        public int LevelCurrent { get; set; } = 1;

        /// <summary>
        /// 目标等级
        /// </summary>
        public int LevelTarget
        {
            get => levelTarget;

            set => Set(ref levelTarget, value);
        }

        /// <summary>
        /// 最大等级
        /// </summary>
        [JsonProperty("max_level")]
        public int MaxLevel { get; set; }

        /// <summary>
        /// 转化到提升差异
        /// </summary>
        /// <returns>提升差异</returns>
        public virtual PromotionDelta ToPromotionDelta()
        {
            return new()
            {
                LevelCurrent = LevelCurrent,
                LevelTarget = LevelTarget,
                Id = Id,
            };
        }
    }
}