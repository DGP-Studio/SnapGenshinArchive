using DGP.Genshin.DataModel.Reccording;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 玩家查询服务
    /// </summary>
    public interface IRecordService
    {
        /// <summary>
        /// 查询玩家信息
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="progress">进度</param>
        /// <returns>查询完成的记录封装</returns>
        Task<Record> GetRecordAsync(string? uid, IProgress<string?> progress);
    }
}