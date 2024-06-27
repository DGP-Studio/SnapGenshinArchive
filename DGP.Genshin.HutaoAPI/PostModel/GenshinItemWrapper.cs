using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.PostModel
{
    /// <summary>
    /// 原神物品包装器
    /// </summary>
    public class GenshinItemWrapper
    {
        /// <summary>
        /// 角色列表
        /// </summary>
        public IEnumerable<HutaoItem>? Avatars { get; set; }

        /// <summary>
        /// 武器列表
        /// </summary>
        public IEnumerable<HutaoItem>? Weapons { get; set; }

        /// <summary>
        /// 圣遗物列表
        /// </summary>
        public IEnumerable<HutaoItem>? Reliquaries { get; set; }
    }
}