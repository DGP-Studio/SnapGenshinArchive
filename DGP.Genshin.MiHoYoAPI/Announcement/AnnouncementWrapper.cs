using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Announcement
{
    /// <summary>
    /// 公告包装器
    /// </summary>
    public class AnnouncementWrapper
    {
        /// <summary>
        /// 类别
        /// </summary>
        [JsonProperty("list")]
        public List<AnnouncementListWrapper>? List { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// 类型列表
        /// </summary>
        [JsonProperty("type_list")]
        public List<AnnouncementType>? TypeList { get; set; }

        /// <summary>
        /// 提醒
        /// </summary>
        [JsonProperty("alert")]
        public bool Alert { get; set; }

        /// <summary>
        /// 提醒Id
        /// </summary>
        [JsonProperty("alert_id")]
        public int AlertId { get; set; }

        /// <summary>
        /// 时区
        /// </summary>
        [JsonProperty("timezone")]
        public int TimeZone { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonProperty("t")]
        public long TimeStamp { get; set; }
    }
}