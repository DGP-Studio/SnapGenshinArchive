namespace DGP.Genshin.DataModel.Achievement.Decomposed
{
    /// <summary>
    /// 分步步骤 步
    /// </summary>
    public class DecomposedStep
    {
        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
    }
}