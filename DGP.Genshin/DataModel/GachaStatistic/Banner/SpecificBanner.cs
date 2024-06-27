using DGP.Genshin.DataModel.GachaStatistic.Item;
using Snap.Data.Primitive;
using Snap.Extenion.Enumerable;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.GachaStatistic.Banner
{
    /// <summary>
    /// 20或14天一轮的up卡池信息
    /// </summary>
    public class SpecificBanner : BannerBase, IPartiallyCloneable<SpecificBanner>
    {
        /// <summary>
        /// 卡池的类型 详见：<see cref="MiHoYoAPI.Gacha.ConfigType"/>
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Up的五星列表
        /// </summary>
        public List<StatisticItem>? UpStar5List { get; set; }

        /// <summary>
        /// Up的四星列表
        /// </summary>
        public List<StatisticItem>? UpStar4List { get; set; }

        /// <summary>
        /// 五星物品计数列表
        /// </summary>
        public List<StatisticItem>? StatisticList5 { get; set; }

        /// <summary>
        /// 四星物品计数列表
        /// </summary>
        public List<StatisticItem>? StatisticList4 { get; set; }

        /// <summary>
        /// 三星物品计数列表
        /// </summary>
        public List<StatisticItem>? StatisticList3 { get; set; }

        /// <summary>
        /// 物品列表
        /// </summary>
        public List<SpecificItem> Items { get; set; } = new();

        /// <summary>
        /// 根据四星切片的列表
        /// </summary>
        public List<List<SpecificItem>> Slices { get; set; } = new();

        /// <inheritdoc/>
        public SpecificBanner ClonePartially()
        {
            return new SpecificBanner
            {
                Type = Type,
                UpStar5List = UpStar5List?.ClonePartially(),
                UpStar4List = UpStar4List?.ClonePartially(),
                CurrentName = CurrentName,
                StartTime = StartTime,
                EndTime = EndTime,
            };
        }

        /// <summary>
        /// 清楚物品列表与五星列表
        /// </summary>
        public void ClearItemsAndStar5List()
        {
            Items?.Clear();
            Star5List?.Clear();
        }
    }
}