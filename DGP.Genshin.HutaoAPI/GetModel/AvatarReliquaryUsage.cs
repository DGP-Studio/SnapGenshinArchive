using System.Collections.Generic;

namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 圣遗物配置数据
    /// </summary>
    public class AvatarReliquaryUsage
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int Avatar { get; set; }

        /// <summary>
        /// 圣遗物比率
        /// </summary>
        public IEnumerable<Rate<string>> ReliquaryUsage { get; set; } = null!;
    }
}