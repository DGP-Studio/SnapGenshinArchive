using System.Collections.Generic;
using System.Linq;

namespace DGP.Genshin.HutaoAPI.GetModel
{
    /// <summary>
    /// 队伍
    /// </summary>
    public class Team
    {
        /// <summary>
        /// 上半
        /// </summary>
        public string UpHalf { get; set; } = null!;

        /// <summary>
        /// 下半
        /// </summary>
        public string DownHalf { get; set; } = null!;

        /// <summary>
        /// 获取上半的信息
        /// </summary>
        /// <returns>上半角色Id列表</returns>
        public IEnumerable<int> GetUp()
        {
            return UpHalf.Split(',').Select(x => int.Parse(x));
        }

        /// <summary>
        /// 获取下半的信息
        /// </summary>
        /// <returns>下半角色Id列表</returns>
        public IEnumerable<int> GetDown()
        {
            return DownHalf.Split(',').Select(x => int.Parse(x));
        }
    }
}