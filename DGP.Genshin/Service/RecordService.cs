using DGP.Genshin.DataModel.Reccording;
using DGP.Genshin.MiHoYoAPI.Record;
using DGP.Genshin.MiHoYoAPI.Record.Avatar;
using DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss;
using DGP.Genshin.Service.Abstraction;
using Snap.Core.DependencyInjection;
using System.Threading.Tasks;

namespace DGP.Genshin.Service
{
    /// <summary>
    /// 玩家记录服务的默认实现
    /// </summary>
    [Service(typeof(IRecordService), InjectAs.Transient)]
    internal class RecordService : IRecordService
    {
        private readonly ICookieService cookieService;

        /// <summary>
        /// 构造一个默认的玩家查询服务
        /// </summary>
        /// <param name="cookieService">cookie服务</param>
        /// <param name="messenger">消息器</param>
        public RecordService(ICookieService cookieService)
        {
            this.cookieService = cookieService;
        }

        /// <inheritdoc/>
        public async Task<Record> GetRecordAsync(string? uid, IProgress<string?> progress)
        {
            // dispatch to new thread
            Record? result = await Task.Run(async () =>
            {
                try
                {
                    Must.NotNull(uid!);
                    RecordProvider recordProvider = new(cookieService.CurrentCookie);

                    string? server = recordProvider.EvaluateUidRegion(uid);
                    Must.NotNull(server!);

                    progress.Report("正在获取 玩家基础统计信息 (1/4)");
                    PlayerInfo? playerInfo = await recordProvider.GetPlayerInfoAsync(uid, server);
                    Must.NotNull(playerInfo!);

                    progress.Report("正在获取 本期深境螺旋信息 (2/4)");
                    SpiralAbyss? spiralAbyss = await recordProvider.GetSpiralAbyssAsync(uid, server, SpiralAbyssType.Current);
                    Must.NotNull(spiralAbyss!);

                    progress.Report("正在获取 上期深境螺旋信息 (3/4)");
                    SpiralAbyss? lastSpiralAbyss = await recordProvider.GetSpiralAbyssAsync(uid, server, SpiralAbyssType.Last);
                    Must.NotNull(lastSpiralAbyss!);

                    progress.Report("正在获取 详细角色信息 (4/4)");
                    DetailedAvatarWrapper? detailedAvatarInfo = await recordProvider.GetDetailAvaterInfoAsync(uid, server, playerInfo);
                    Must.NotNull(detailedAvatarInfo!);

                    return new Record
                    {
                        Success = true,
                        UserId = uid,
                        PlayerInfo = playerInfo,
                        SpiralAbyss = spiralAbyss,
                        LastSpiralAbyss = lastSpiralAbyss,
                        DetailedAvatars = detailedAvatarInfo.Avatars,
                    };
                }
                catch (ArgumentNullException ex)
                {
                    return new Record(ex.Message);
                }
                finally
                {
                    progress.Report(null);
                }
            });
            return result;
        }
    }
}