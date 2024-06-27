using Snap.Data.Primitive;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction.IntegrityCheck
{
    /// <summary>
    /// 完整性检查服务
    /// </summary>
    public interface IIntegrityCheckService
    {
        /// <summary>
        /// 封装完整性检查进度报告
        /// </summary>
        public interface IIntegrityCheckState
        {
            /// <summary>
            /// 当前检查进度
            /// </summary>
            int CurrentCount { get; }

            /// <summary>
            /// 总进度
            /// </summary>
            int TotalCount { get; }

            /// <summary>
            /// 描述
            /// </summary>
            string? Info { get; }
        }

        /// <summary>
        /// 完整性检查监视器
        /// </summary>
        WorkWatcher IntegrityChecking { get; }

        /// <summary>
        /// 检查基础缓存图片完整性，不完整的自动下载补全
        /// </summary>
        /// <param name="progress">进度</param>
        /// <returns>任务</returns>
        Task CheckMetadataIntegrityAsync(IProgress<IIntegrityCheckState> progress);
    }
}