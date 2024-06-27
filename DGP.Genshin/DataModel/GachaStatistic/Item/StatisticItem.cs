using Snap.Data.Primitive;

namespace DGP.Genshin.DataModel.GachaStatistic.Item
{
    /// <summary>
    /// 带有个数统计的奖池统计物品
    /// </summary>
    public class StatisticItem : SpecificItem, IPartiallyCloneable<StatisticItem>
    {
        /// <summary>
        /// 个数
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// 隐藏了数量的克隆
        /// </summary>
        /// <returns>克隆，隐藏了数量</returns>
        public StatisticItem ClonePartially()
        {
            return new()
            {
                StarUrl = StarUrl,
                Source = Source,
                Name = Name,
                Badge = Badge,
                Time = Time,
            };
        }
    }
}