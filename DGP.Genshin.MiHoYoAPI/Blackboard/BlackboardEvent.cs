using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Blackboard
{
    public class BlackboardEvent
    {
        [JsonProperty("title")] public string? Title { get; set; }
        /// <summary>
        /// 1 限时活动
        /// 2 日常材料
        /// 4 角色生日
        /// </summary>
        [JsonProperty("kind")] public string? Kind { get; set; }
        [JsonProperty("img_url")] public string? ImgUrl { get; set; }
        [JsonProperty("jump_type")] public string? JumpType { get; set; }
        [JsonProperty("jump_url")] public string? JumpUrl { get; set; }
        [JsonProperty("content_id")] public string? ContentId { get; set; }
        [JsonProperty("style")] public string? Style { get; set; }
        [JsonProperty("start_time")] public string? StartTime { get; set; }
        [JsonProperty("end_time")] public string? EndTime { get; set; }
        [JsonProperty("font_color")] public string? FontColor { get; set; }
        [JsonProperty("padding_color")] public string? PaddingColor { get; set; }
        /// <summary>
        /// 掉落的星期几
        /// 1~7
        /// </summary>
        [JsonProperty("drop_day")] public List<string>? DropDay { get; set; }
        [JsonProperty("break_type")] public string? BreakType { get; set; }
        [JsonProperty("id")] public string? Id { get; set; }
        [JsonProperty("contentInfos")] public List<ContentInfo>? ContentInfos { get; set; }
        [JsonProperty("sort")] public string? Sort { get; set; }

        public DateTime? StartDateTime
        {
            get
            {
                if (StartTime is not null)
                {
                    DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds(int.Parse(StartTime));
                    return dto.ToLocalTime().DateTime;
                }
                return null;
            }
        }
        public DateTime? EndDateTime
        {
            get
            {
                if (EndTime is not null)
                {
                    DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds(int.Parse(EndTime));
                    return dto.ToLocalTime().DateTime;
                }
                return null;
            }
        }
    }
}
