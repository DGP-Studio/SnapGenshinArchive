namespace DGP.Genshin.DataModel.Achievement.CocoGoat
{
    /// <summary>
    /// 椰羊成就数据
    /// </summary>
    public class CocoGoatAchievement
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 完成状态
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public int CategoryId { get; set; }
    }
}