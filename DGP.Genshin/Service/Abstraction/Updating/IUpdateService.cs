using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction.Updating
{
    /// <summary>
    /// 更新服务
    /// </summary>
    public interface IUpdateService
    {
        /// <summary>
        /// 当前App版本
        /// </summary>
        Version CurrentVersion { get; }

        /// <summary>
        /// 更新API获取的App版本
        /// </summary>
        Version? NewVersion { get; }

        /// <summary>
        /// 下载更新包的Url
        /// </summary>
        Uri? PackageUri { get; }

        /// <summary>
        /// 发行日志
        /// </summary>
        string? ReleaseNote { get; }

        /// <summary>
        /// 异步检查更新
        /// </summary>
        /// <returns>更新状态</returns>
        Task<UpdateState> CheckUpdateStateAsync();

        /// <summary>
        /// 下载并安装更新包
        /// 尽量避免在捕获的上下文中使用
        /// </summary>
        /// <returns>任务</returns>
        Task DownloadAndInstallPackageAsync();
    }
}