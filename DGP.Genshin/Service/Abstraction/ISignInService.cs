using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 签到服务
    /// </summary>
    public interface ISignInService
    {
        /// <summary>
        /// 尝试签到所有角色
        /// </summary>
        /// <returns>任务</returns>
        Task TrySignAllAccountsRolesInAsync();
    }
}