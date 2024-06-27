namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 排行包装
    /// </summary>
    public class RankWrapper
    { 
        /// <summary>
        /// 伤害
        /// </summary>
        public Rank? Damage { get; set; }

        /// <summary>
        /// 承受伤害
        /// </summary>
        public Rank? TakeDamage { get; set; }
    }
}