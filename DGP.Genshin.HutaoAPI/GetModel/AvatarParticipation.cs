using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 出场数据
    /// </summary>
    public class AvatarParticipation
    {
        /// <summary>
        /// 层
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// 角色比率
        /// </summary>
        public IEnumerable<Rate<int>> AvatarUsage { get; set; } = null!;
    }
}