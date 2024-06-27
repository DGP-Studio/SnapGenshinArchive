using DGP.Genshin.DataModel.GachaStatistic.Item;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.GachaStatistic.Banner
{
    /// <summary>
    /// 卡池共有基本信息
    /// </summary>
    public abstract class BannerBase
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 当前展示名称
        /// </summary>
        public string? CurrentName { get; set; }

        /// <summary>
        /// 五星物品计数列表
        /// 此处的计数用于计算距离上一个五星的个数
        /// </summary>
        public List<StatisticItem5Star>? Star5List { get; set; }

        /// <summary>
        /// 五星计数
        /// </summary>
        public int Star5Count { get; set; }

        /// <summary>
        /// 四星计数
        /// </summary>
        public int Star4Count { get; set; }

        /// <summary>
        /// 三星计数
        /// </summary>
        public int Star3Count { get; set; }
    }
}