using System.Threading;

namespace DGP.Genshin.Control.Infrastructure.Concurrent
{
    /// <summary>
    /// 指示此类支持取消任务
    /// </summary>
    public interface ISupportCancellation
    {
        /// <summary>
        /// 用于通知取消的取消回执
        /// </summary>
        CancellationToken CancellationToken { get; set; }
    }
}