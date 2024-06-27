namespace DGP.Genshin.Service.Abstraction.Updating
{
    /// <summary>
    /// 更新状态枚举
    /// </summary>
    public enum UpdateState
    {
        /// <summary>
        /// 需要更新
        /// </summary>
        NeedUpdate = 0,

        /// <summary>
        /// 最新版本
        /// </summary>
        IsNewestRelease = 1,

        /// <summary>
        /// 内部开发版本
        /// </summary>
        IsInsiderVersion = 2,

        /// <summary>
        /// 更新不可用
        /// </summary>
        NotAvailable = 3,
    }
}