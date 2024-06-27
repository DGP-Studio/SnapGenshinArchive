using DGP.Genshin.DataModel.GachaStatistic.Banner;
using DGP.Genshin.DataModel.GachaStatistic.Item;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.GachaStatistic
{
    /// <summary>
    /// 统计信息
    /// </summary>
    public class Statistic
    {
        /// <summary>
        /// Uid
        /// </summary>
        public string? Uid { get; set; }

        /// <summary>
        /// 常驻池
        /// </summary>
        public StatisticBanner? Permanent { get; set; }

        /// <summary>
        /// 角色祈愿
        /// </summary>
        public StatisticBanner? CharacterEvent { get; set; }

        /// <summary>
        /// 武器祈愿
        /// </summary>
        public StatisticBanner? WeaponEvent { get; set; }

        /// <summary>
        /// 五星角色列表
        /// </summary>
        public List<StatisticItem>? Characters5 { get; set; }

        /// <summary>
        /// 四星角色列表
        /// </summary>
        public List<StatisticItem>? Characters4 { get; set; }

        /// <summary>
        /// 五星武器列表
        /// </summary>
        public List<StatisticItem>? Weapons5 { get; set; }

        /// <summary>
        /// 四星武器列表
        /// </summary>
        public List<StatisticItem>? Weapons4 { get; set; }

        /// <summary>
        /// 三星武器列表
        /// </summary>
        public List<StatisticItem>? Weapons3 { get; set; }

        /// <summary>
        /// 历史卡池列表
        /// </summary>
        public List<SpecificBanner>? SpecificBanners { get; set; }
    }
}