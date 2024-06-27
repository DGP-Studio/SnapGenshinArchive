using DGP.Genshin.MiHoYoAPI.Announcement;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 主页服务
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// 异步获取游戏公告与活动
        /// </summary>
        /// <param name="openAnnouncementUICommand">打开公告所需的命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>公告包装器</returns>
        Task<AnnouncementWrapper> GetAnnouncementsAsync(ICommand openAnnouncementUICommand, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取 Snap Genshin 官方公告
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>公告文本</returns>
        Task<string> GetManifestoAsync(CancellationToken cancellationToken = default);
    }
}