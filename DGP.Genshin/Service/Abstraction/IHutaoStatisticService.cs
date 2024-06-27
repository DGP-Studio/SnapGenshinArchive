using DGP.Genshin.DataModel.HutaoAPI;
using DGP.Genshin.HutaoAPI.GetModel;
using DGP.Genshin.HutaoAPI.PostModel;
using DGP.Genshin.MiHoYoAPI.Response;
using Snap.Data.Primitive;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction
{
    /// <summary>
    /// 胡桃数据库服务
    /// </summary>
    public interface IHutaoStatisticService
    {
        /// <summary>
        /// 获取并上传用户当前的数据
        /// </summary>
        /// <param name="cookie">用户的cookie</param>
        /// <param name="confirmFunc">确认操作回调</param>
        /// <param name="resultAsyncFunc">结果异步操作回调</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        Task GetAllRecordsAndUploadAsync(string cookie, Func<PlayerRecord, Task<bool>> confirmFunc, Func<Response, Task> resultAsyncFunc, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取角色命座
        /// </summary>
        /// <returns>角色命座</returns>
        IList<Rate<Item<IList<NamedValue<double>>>>> GetAvatarConstellations();

        /// <summary>
        /// 获取角色使用率
        /// </summary>
        /// <returns>角色使用率</returns>
        IList<Indexed<int, Item<double>>> GetAvatarParticipation2s();

        /// <summary>
        /// 获取人数总览
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>总览数据</returns>
        Task<Overview?> GetOverviewAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取uid是否在当期上传了数据
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>当期是否上传了数据</returns>
        Task<bool> GetPeriodUploadedAsync(string uid, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 异步获取角色排行
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色排行</returns>
        Task<Two<Item<Rank>>?> GetRankAsync(string uid, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 获取圣遗物搭配
        /// </summary>
        /// <returns>圣遗物搭配</returns>
        IList<Item<IList<NamedValue<Rate<IList<Item<int>>>>>>> GetReliquaryUsages();

        /// <summary>
        /// 获取角色搭配
        /// </summary>
        /// <returns>角色搭配</returns>
        IList<Item<IList<Item<double>>>> GetTeamCollocations();

        /// <summary>
        /// 获取队伍出场次数
        /// </summary>
        /// <returns>队伍出场次数</returns>
        IList<Indexed<int, Rate<Two<IList<HutaoItem>>>>> GetTeamCombinations();

        /// <summary>
        /// 获取武器使用率
        /// </summary>
        /// <returns>武器使用率</returns>
        IList<Item<IList<Item<double>>>> GetWeaponUsages();

        /// <summary>
        /// 异步初始化
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}