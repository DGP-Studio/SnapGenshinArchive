namespace DGP.Genshin.DataModel.GachaStatistic.Item
{
    /// <summary>
    /// 用于展示的卡池物品
    /// </summary>
    public class SpecificItem
    {
        /// <summary>
        /// 稀有度Url
        /// </summary>
        public string? StarUrl { get; set; }

        /// <summary>
        /// 物品图片Url
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// 物品名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 角标Url
        /// </summary>
        public string? Badge { get; set; }

        /// <summary>
        /// 获得时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Time}-{Name}";
        }

        /// <summary>
        /// 从元件中复制信息
        /// </summary>
        /// <param name="primitive">原件</param>
        public void CopyFromPrimitive(Primitive primitive)
        {
            StarUrl = primitive.Star;
            Source = primitive.Source;
            Name = primitive.Name;
            Badge = primitive.GetBadge();
        }
    }
}