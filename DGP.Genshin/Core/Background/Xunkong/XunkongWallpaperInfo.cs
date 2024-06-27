using Newtonsoft.Json;

namespace DGP.Genshin.Core.Background.Xunkong
{
    internal record XunkongWallpaperInfo
    {
        /// <summary>
        /// 图片Url
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; set; }
    }
}