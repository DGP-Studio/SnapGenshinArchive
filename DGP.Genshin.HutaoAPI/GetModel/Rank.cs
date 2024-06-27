namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 排行
    /// </summary>
    public class Rank
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public double Percent { get; set; }

        /// <summary>
        /// 总体百分比
        /// </summary>
        public double PercentTotal { get; set; }
    }
}