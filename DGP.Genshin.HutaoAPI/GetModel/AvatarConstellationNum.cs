using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 命座比例
    /// </summary>
    public class AvatarConstellationNum
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int Avatar { get; set; }

        /// <summary>
        /// 持有率
        /// </summary>
        public double HoldingRate { get; set; }

        /// <summary>
        /// 各命座比率
        /// </summary>
        public IEnumerable<Rate<int>> Rate { get; set; } = null!;
    }
}