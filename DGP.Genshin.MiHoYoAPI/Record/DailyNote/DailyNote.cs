using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record.DailyNote
{
    public class DailyNote
    {
        /// <summary>
        /// 当前树脂
        /// </summary>
        [JsonProperty("current_resin")]
        public int CurrentResin { get; set; }

        /// <summary>
        /// 最大树脂
        /// </summary>
        [JsonProperty("max_resin")]
        public int MaxResin { get; set; }

        /// <summary>
        /// 树脂恢复时间 <see cref="string"/>类型的秒数
        /// </summary>
        [JsonProperty("resin_recovery_time")]
        public string? ResinRecoveryTime { get; set; }

        /// <summary>
        /// 格式化的树脂恢复时间
        /// </summary>
        public string? ResinRecoveryTargetTimeFormatted
        {
            get
            {
                if (ResinRecoveryTime is not null)
                {
                    DateTime tt = DateTime.Now.AddSeconds(int.Parse(ResinRecoveryTime));
                    int totalDays = (tt - DateTime.Today).Days;
                    string day = totalDays switch
                    {
                        0 => "今天",
                        1 => "明天",
                        2 => "后天",
                        _ => $"{totalDays}天",
                    };
                    return $"{day} {tt:HH:mm}";
                }

                return null;
            }
        }

        /// <summary>
        /// 委托完成数
        /// </summary>
        [JsonProperty("finished_task_num")]
        public int FinishedTaskNum { get; set; }

        /// <summary>
        /// 委托总数
        /// </summary>
        [JsonProperty("total_task_num")]
        public int TotalTaskNum { get; set; }

        /// <summary>
        /// 4次委托额外奖励是否领取
        /// </summary>
        [JsonProperty("is_extra_task_reward_received")]
        public bool IsExtraTaskRewardReceived { get; set; }

        /// <summary>
        /// 每日委托奖励字符串
        /// </summary>
        public string ExtraTaskRewardDescription
        {
            get
            {
                return IsExtraTaskRewardReceived
                    ? "已领取「每日委托」奖励"
                    : FinishedTaskNum == TotalTaskNum
                        ? "「每日委托」奖励待领取"
                        : "今日完成委托次数不足";
            }
        }

        /// <summary>
        /// 剩余周本折扣次数
        /// </summary>
        [JsonProperty("remain_resin_discount_num")]
        public int RemainResinDiscountNum { get; set; }

        /// <summary>
        /// 周本树脂减免使用次数
        /// </summary>
        public int ResinDiscountUsedNum
        {
            get => ResinDiscountNumLimit - RemainResinDiscountNum;
        }

        /// <summary>
        /// 周本折扣总次数
        /// </summary>
        [JsonProperty("resin_discount_num_limit")]
        public int ResinDiscountNumLimit { get; set; }

        /// <summary>
        /// 当前派遣数
        /// </summary>
        [JsonProperty("current_expedition_num")]
        public int CurrentExpeditionNum { get; set; }

        /// <summary>
        /// 最大派遣数
        /// </summary>
        [JsonProperty("max_expedition_num")]
        public int MaxExpeditionNum { get; set; }

        /// <summary>
        /// 派遣
        /// </summary>
        [JsonProperty("expeditions")]
        public List<Expedition>? Expeditions { get; set; }

        /// <summary>
        /// 当前洞天宝钱
        /// </summary>
        [JsonProperty("current_home_coin")]
        public int CurrentHomeCoin { get; set; }

        /// <summary>
        /// 最大洞天宝钱
        /// </summary>
        [JsonProperty("max_home_coin")]
        public int MaxHomeCoin { get; set; }

        /// <summary>
        /// 洞天宝钱恢复时间 <see cref="string"/>类型的秒数
        /// </summary>
        [JsonProperty("home_coin_recovery_time")]
        public string? HomeCoinRecoveryTime { get; set; }

        /// <summary>
        /// 格式化的洞天宝钱恢复时间
        /// </summary>
        public string? HomeCoinRecoveryTargetTimeFormatted
        {
            get
            {
                if (HomeCoinRecoveryTime is not null)
                {
                    DateTime tt = DateTime.Now.AddSeconds(int.Parse(HomeCoinRecoveryTime));
                    int totalDays = (tt - DateTime.Today).Days;
                    string day = totalDays switch
                    {
                        0 => "今天",
                        1 => "明天",
                        2 => "后天",
                        _ => $"{totalDays}天",
                    };
                    return $"{day} {tt:HH:mm}";
                }

                return null;
            }
        }

        /// <summary>
        /// 参量质变仪
        /// </summary>
        [JsonProperty("transformer")]
        public Transformer? Transformer { get; set; }
    }
}