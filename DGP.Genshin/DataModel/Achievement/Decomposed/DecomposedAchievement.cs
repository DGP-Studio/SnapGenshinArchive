using Snap.Data.Primitive;
using System.Collections.Generic;

namespace DGP.Genshin.DataModel.Achievement.Decomposed
{
    /// <summary>
    /// 分步描述
    /// </summary>
    public class DecomposedAchievement : Observable
    {
        private List<DecomposedStep>? states;

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 成就Id
        /// </summary>
        public int AchievementId { get; set; }

        /// <summary>
        /// 步骤标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 相关的每日
        /// </summary>
        public List<string>? Daily { get; set; }

        /// <summary>
        /// 分步
        /// </summary>
        public List<string>? Decomposed { get; set; }

        /// <summary>
        /// 分步的状态
        /// </summary>
        public List<DecomposedStep>? Steps { get => states; set => Set(ref states, value); }
    }
}