using DGP.Genshin.Service.Abstraction.IntegrityCheck;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 元数据更新服务
    /// </summary>
    public interface IMetadataService
    {
        /// <summary>
        /// meta文件是否存在
        /// </summary>
        bool IsMetaPresent { get; }

        /// <summary>
        /// 尝试保证数据的时效性
        /// </summary>
        /// <param name="progress">进度</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功</returns>
        Task<bool> TryEnsureDataNewestAsync(IProgress<IIntegrityCheckService.IIntegrityCheckState>? progress, CancellationToken cancellationToken = default(CancellationToken));
    }
}