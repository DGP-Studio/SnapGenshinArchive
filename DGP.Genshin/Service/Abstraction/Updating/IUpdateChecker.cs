using DGP.Genshin.DataModel.Updating;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction.Updating
{
    /// <summary>
    /// 更新检查器接口
    /// </summary>
    public interface IUpdateChecker
    {
        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <returns>更新信息</returns>
        public Task<UpdateInfomation?> GetUpdateInfomationAsync();
    }
}