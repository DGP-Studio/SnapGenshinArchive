using DGP.Genshin.MiHoYoAPI.Gacha;
using DGP.Genshin.Service.GachaStatistic;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction.GachaStatistic
{
    /// <summary>
    /// 联机抽卡记录工作器
    /// </summary>
    public interface IGachaLogWorker
    {
        /// <summary>
        /// 延迟范围，单位毫秒
        /// </summary>
        (int Min, int Max) DelayRange { get; set; }

        /// <summary>
        /// 设置祈愿接口获取延迟是否启用
        /// </summary>
        bool IsFetchDelayEnabled { get; set; }

        /// <summary>
        /// 当前处理的祈愿记录
        /// </summary>
        GachaDataCollection WorkingGachaData { get; set; }

        /// <summary>
        /// 当前处理的祈愿记录的Uid
        /// </summary>
        string? WorkingUid { get; }

        /// <summary>
        /// 获取单个奖池的祈愿记录全量信息
        /// 并自动合并数据
        /// </summary>
        /// <param name="type">卡池类型</param>
        /// <param name="progress">进度</param>
        /// <returns>uid 失败时为 <see langword="null"/></returns>
        Task<string?> FetchGachaLogAggressivelyAsync(ConfigType type, IProgress<FetchProgress> progress);

        /// <summary>
        /// 获取单个奖池的祈愿记录增量信息
        /// 并自动合并数据
        /// </summary>
        /// <param name="type">卡池类型</param>
        /// <param name="progress">进度</param>
        /// <returns>uid 失败时为 <see langword="null"/></returns>
        Task<string?> FetchGachaLogIncreaselyAsync(ConfigType type, IProgress<FetchProgress> progress);

        /// <summary>
        /// 获取当前的卡池配置
        /// </summary>
        /// <returns>卡池配置</returns>
        Task<Config?> GetCurrentGachaConfigAsync();

        /// <summary>
        /// 获取随机延迟的时间长度
        /// </summary>
        /// <returns>随机的延迟长度</returns>
        int GetRandomDelay();
    }
}