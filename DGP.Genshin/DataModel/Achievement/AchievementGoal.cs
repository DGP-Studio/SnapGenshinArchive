namespace DGP.Genshin.DataModel.Achievement
{
    /// <summary>
    /// 成就大纲
    /// </summary>
    internal class AchievementGoal
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 顺序Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 图标路径
        /// </summary>
        public string? IconPath { get; set; }
    }
}