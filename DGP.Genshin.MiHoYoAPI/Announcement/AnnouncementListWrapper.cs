using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Announcement
{
    /// <summary>
    /// 公告列表
    /// </summary>
    public class AnnouncementListWrapper
    {
        /// <summary>
        /// 列表
        /// </summary>
        [JsonProperty("list")]
        public List<Announcement>? List { get; set; }

        /// <summary>
        /// 类型Id
        /// </summary>
        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        /// <summary>
        /// 类型标签
        /// </summary>
        [JsonProperty("type_label")]
        public string? TypeLabel { get; set; }
    }
}