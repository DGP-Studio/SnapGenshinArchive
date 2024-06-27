using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Achievement.UIAF
{
    /// <summary>
    /// 封装 UIAF 标准对象
    /// </summary>
    public class UIAF
    {
        /// <summary>
        /// 信息
        /// </summary>
        [JsonProperty("info")]
        public UIAFInfo? Info { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        [JsonProperty("list")]
        public IEnumerable<UIAFItem>? List { get; set; }
    }
}