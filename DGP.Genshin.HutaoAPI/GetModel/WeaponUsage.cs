using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 武器使用数据
    /// </summary>
    public class WeaponUsage
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int Avatar { get; set; }

        /// <summary>
        /// 武器比率
        /// </summary>
        public IEnumerable<Rate<int>> Weapons { get; set; } = null!;
    }
}