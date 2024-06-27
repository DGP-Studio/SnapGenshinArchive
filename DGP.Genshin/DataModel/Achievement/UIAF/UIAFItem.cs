using Newtonsoft.Json;

namespace DGP.Genshin.DataModel.Achievement.UIAF
{
    /// <summary>
    /// UIAF的列表项
    /// </summary>
    public class UIAFItem
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }

        /// <summary>
        /// 当前进度
        /// </summary>
        [JsonProperty("current")]
        public int? Current { get; set; }

        /// <summary>
        /// 完成状态
        /// </summary>
        public AchievementInfoStatus Status { get; set; }
    }

    /// <summary>
    /// 成就信息状态
    /// https://github.com/Grasscutters/Grasscutter/blob/development/proto/AchievementInfo.proto
    /// </summary>
    public enum AchievementInfoStatus
    {
        /// <summary>
        /// 非法值
        /// </summary>
        ACHIEVEMENT_INVALID = 0,

        /// <summary>
        /// 未完成
        /// </summary>
        ACHIEVEMENT_UNFINISHED = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        ACHIEVEMENT_FINISHED = 2,

        /// <summary>
        /// 奖励已领取
        /// </summary>
        ACHIEVEMENT_POINT_TAKEN = 3,
    }
}