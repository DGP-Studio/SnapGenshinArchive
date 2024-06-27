﻿using Newtonsoft.Json;
using System;
using System.Windows.Input;

namespace DGP.Genshin.MiHoYoAPI.Announcement
{
    /// <summary>
    /// 公告
    /// </summary>
    public class Announcement : AnnouncementContent
    {
        private double timePercent;

        /// <summary>
        /// 类型标签
        /// </summary>
        [JsonProperty("type_label")]
        public string? TypeLabel { get; set; }

        /// <summary>
        /// 标签文本
        /// </summary>
        [JsonProperty("tag_label")]
        public string? TagLabel { get; set; }

        /// <summary>
        /// 标签图标
        /// </summary>
        [JsonProperty("tag_icon")]
        public string? TagIcon { get; set; }

        /// <summary>
        /// 登录提醒
        /// </summary>
        [JsonProperty("login_alert")]
        public int LoginAlert { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 启动展示窗口的命令
        /// </summary>
        public ICommand? OpenAnnouncementUICommand { get; set; }

        /// <summary>
        /// 是否应展示时间
        /// </summary>
        public bool ShouldShowTimeDescription
        {
            get => Type == 1;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public string TimeDescription
        {
            get
            {
                DateTime now = DateTime.UtcNow + TimeSpan.FromHours(8);

                // 尚未开始
                if (StartTime > now)
                {
                    TimeSpan span = StartTime - now;
                    if (span.TotalDays <= 1)
                    {
                        return $"{(int)span.TotalHours} 小时后开始";
                    }

                    return $"{(int)span.TotalDays} 天后开始";
                }
                else
                {
                    TimeSpan span = EndTime - now;
                    if (span.TotalDays <= 1)
                    {
                        return $"{(int)span.TotalHours} 小时后结束";
                    }

                    return $"{(int)span.TotalDays} 天后结束";
                }
            }
        }

        /// <summary>
        /// 是否应显示时间百分比
        /// </summary>
        public bool ShouldShowTimePercent
        {
            get => ShouldShowTimeDescription && (TimePercent > 0 && TimePercent < 1);
        }

        /// <summary>
        /// 时间百分比
        /// </summary>
        public double TimePercent
        {
            get
            {
                if (timePercent == 0)
                {
                    // UTC+8
                    DateTime currentTime = DateTime.UtcNow.AddHours(8);
                    TimeSpan current = currentTime - StartTime;
                    TimeSpan total = EndTime - StartTime;
                    timePercent = current / total;
                }

                return timePercent;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// 提醒
        /// </summary>
        [JsonProperty("remind")]
        public int Remind { get; set; }

        /// <summary>
        /// 通知
        /// </summary>
        [JsonProperty("alert")]
        public int Alert { get; set; }

        /// <summary>
        /// 标签开始时间
        /// </summary>
        [JsonProperty("tag_start_time")]
        public string? TagStartTime { get; set; }

        /// <summary>
        /// 标签结束时间
        /// </summary>
        [JsonProperty("tag_end_time")]
        public string? TagEndTime { get; set; }

        /// <summary>
        /// 提醒版本
        /// </summary>
        [JsonProperty("remind_ver")]
        public int RemindVer { get; set; }

        /// <summary>
        /// 是否含有内容
        /// </summary>
        [JsonProperty("has_content")]
        public bool HasContent { get; set; }
    }
}