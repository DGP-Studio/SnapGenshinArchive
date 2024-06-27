namespace DGP.Genshin.DataModel.GachaStatistic.Banner
{
    /// <summary>
    /// 单个卡池统计信息
    /// </summary>
    public class StatisticBanner : ProbabilityBanner
    {
        /// <summary>
        /// 据上一次出五星
        /// </summary>
        public int CountSinceLastStar5 { get; set; }

        /// <summary>
        /// 据上一次出四星
        /// </summary>
        public int CountSinceLastStar4 { get; set; }

        /// <summary>
        /// 五星平均抽数
        /// </summary>
        public double AverageGetStar5 { get; set; }

        /// <summary>
        /// 最非抽数
        /// </summary>
        public int MaxGetStar5Count { get; set; }

        /// <summary>
        /// 最欧抽数
        /// </summary>
        public int MinGetStar5Count { get; set; }

        /// <summary>
        /// 下一个保底类型
        /// </summary>
        public string? NextGuaranteeType { get; set; }
    }
}