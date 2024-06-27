namespace DGP.Genshin.Service.Abstraction.Updating
{
    /// <summary>
    /// 更新API
    /// </summary>
    public enum UpdateAPI
    {
        /// <summary>
        /// 使用 Github API 检查更新
        /// </summary>
        GithubAPI = 0,

        /// <summary>
        /// 使用 Patch API 检查更新
        /// </summary>
        PatchAPI = 1,
    }
}