using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 队伍上场率
    /// </summary>
    public record TeamCombination2
    {
        /// <summary>
        /// 层
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// 队伍
        /// </summary>
        public IEnumerable<Rate<Team>> Teams { get; set; } = null!;
    }
}