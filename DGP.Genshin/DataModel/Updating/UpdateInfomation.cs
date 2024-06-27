using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Updating
{
    /// <summary>
    /// 更新信息
    /// </summary>
    public class UpdateInfomation
    {
        /// <summary>
        /// 更新日志
        /// </summary>
        [JsonProperty("body")]
        public string? ReleaseNote { get; set; }

        /// <summary>
        /// 更新包Url
        /// </summary>
        [JsonProperty("browser_download_url")]
        public Uri? PackageUrl { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("tag_name")]
        public string Version { get; set; } = null!;
    }
}