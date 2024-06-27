using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 队伍上场率
    /// </summary>
    public record TeamCombination
    {
        /// <summary>
        /// 间
        /// </summary>
        public Level Level { get; set; } = null!;

        /// <summary>
        /// 队伍
        /// </summary>
        public IEnumerable<Rate<Team>> Teams { get; set; } = null!;
    }
}