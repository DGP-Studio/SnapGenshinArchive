namespace DGP.Genshin.DataModel.GachaStatistic.Banner
{
    /// <summary>
    /// 单个卡池信息
    /// 带有额外的3个星级的概率
    /// </summary>
    public abstract class ProbabilityBanner : BannerBase
    {
        /// <summary>
        /// 五星概率
        /// </summary>
        public double Star5Prob { get; set; }

        /// <summary>
        /// 四星概率
        /// </summary>
        public double Star4Prob { get; set; }

        /// <summary>
        /// 三星概率
        /// </summary>
        public double Star3Prob { get; set; }
    }
}