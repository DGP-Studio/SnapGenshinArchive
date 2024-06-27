using DGP.Genshin.DataModel.Achievement.Decomposed;
using Snap.Data.Primitive;

namespace DGP.Genshin.DataModel.Achievement
{
    /// <summary>
    /// 成就
    /// </summary>
    public class Achievement : Observable
    {
        private bool isCompleted;
        private DateTime completeDateTime;

        /// <summary>
        /// 大纲Id
        /// </summary>
        public int GoalId { get; set; }

        /// <summary>
        /// 前置成就Id
        /// </summary>
        public int PreStageAchievementId { get; set; }

        /// <summary>
        /// 顺序Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 奖励原石个数
        /// </summary>
        public int FinishRewardCount { get; set; }

        /// <summary>
        /// 是否弃用
        /// </summary>
        public bool IsDisuse { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 是否完成
        /// 初始化时，请注意先设置 <see cref="CompleteDateTime"/> 的值
        /// </summary>
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                Set(ref isCompleted, value);

                if (isCompleted)
                {
                    if (CompleteDateTime == default(DateTime))
                    {
                        CompleteDateTime = DateTime.Now;
                    }
                }
                else
                {
                    CompleteDateTime = default(DateTime);
                }
            }
        }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompleteDateTime { get => completeDateTime; set => Set(ref completeDateTime, value); }

        /// <summary>
        /// 步骤
        /// </summary>
        public DecomposedAchievement? Decomposition { get; set; }
    }
}