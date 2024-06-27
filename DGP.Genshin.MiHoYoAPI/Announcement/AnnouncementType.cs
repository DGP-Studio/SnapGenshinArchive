using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Announcement
{
    /// <summary>
    /// 公告类型
    /// </summary>
    public class AnnouncementType
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 国际化名称
        /// </summary>
        [JsonProperty("mi18n_name")]
        public string? MI18NName { get; set; }
    }
}