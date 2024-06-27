using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 定时计划服务
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// 异步初始化服务
        /// </summary>
        /// <returns>任务</returns>
        Task InitializeAsync();

        /// <summary>
        /// 终止服务
        /// </summary>
        void UnInitialize();
    }
}