using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    /// <summary>
    /// 更多信息详见 https://github.com/DGP-Studio/Snap.Genshin/wiki/StandardFormat
    /// </summary>
    public class UIGF
    {
        /// <summary>
        /// 信息
        /// </summary>
        [JsonProperty("info")]
        public UIGFInfo? Info { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        [JsonProperty("list")]
        public IEnumerable<UIGFItem?>? List { get; set; }
    }
}