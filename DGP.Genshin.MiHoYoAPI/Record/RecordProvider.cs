using DGP.Genshin.MiHoYoAPI.Record.Avatar;
using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Request.DynamicSecret;
using Microsoft;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    public class RecordProvider
    {
        private const string ApiTakumiRecord = @"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api";
        private const string Referer = @"https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";

        private readonly Requester requester;

        /// <summary>
        /// 使用同一个提供器可用重复请求
        /// </summary>
        /// <param name="cookie"></param>
        public RecordProvider(string cookie)
        {
            requester = new(new()
            {
                { "Accept", RequestOptions.Json },
                { "x-rpc-app_version", DynamicSecretProvider2.AppVersion },
                { "User-Agent", RequestOptions.CommonUA2352 },
                { "x-rpc-client_type", "5" },
                { "Referer", Referer },
                { "Cookie", cookie },
                { "X-Requested-With", RequestOptions.Hyperion }
            });
        }

        /// <summary>
        /// 解析玩家服务器
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string? EvaluateUidRegion(string? uid)
        {
            return string.IsNullOrEmpty(uid)
                ? null
                : uid[0] switch
                {
                    >= '1' and <= '4' => "cn_gf01",
                    '5' => "cn_qd01",
                    '6' => "os_usa",
                    '7' => "os_euro",
                    '8' => "os_asia",
                    '9' => "os_cht",
                    _ => null
                };
        }

        /// <summary>
        /// 获取玩家基础信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<PlayerInfo?> GetPlayerInfoAsync(string uid, string server, CancellationToken cancellationToken = default)
        {
            return await requester.GetWhileUpdateDynamicSecret2Async<PlayerInfo>(
                $@"{ApiTakumiRecord}/index?server={server}&role_id={uid}", cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 获取玩家深渊信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <param name="type">1：当期，2：上期</param>
        /// <returns></returns>
        public async Task<SpiralAbyss.SpiralAbyss?> GetSpiralAbyssAsync(string uid, string server, int type, CancellationToken cancellationToken = default)
        {
            return await requester.GetWhileUpdateDynamicSecret2Async<SpiralAbyss.SpiralAbyss>(
                $@"{ApiTakumiRecord}/spiralAbyss?schedule_type={type}&server={server}&role_id={uid}", cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SpiralAbyss.SpiralAbyss?> GetSpiralAbyssAsync(string uid, string server, SpiralAbyssType type, CancellationToken cancellationToken = default)
        {
            return await GetSpiralAbyssAsync(uid, server, (int)type, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 获取玩家活动信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<object?> GetActivitiesAsync(string uid, string server, CancellationToken cancellationToken = default)
        {
            return await requester.GetWhileUpdateDynamicSecret2Async<object>(
                $@"{ApiTakumiRecord}/activities?server={server}&role_id={uid}", cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// 获取玩家角色详细信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <param name="playerInfo">玩家的基础信息</param>
        /// <returns></returns>
        [SuppressMessage("", "IDE0037")]
        public async Task<DetailedAvatarWrapper?> GetDetailAvaterInfoAsync(string uid, string server, PlayerInfo playerInfo, CancellationToken cancellationToken = default)
        {
            List<Avatar.Avatar>? avatars = playerInfo.Avatars;
            Requires.NotNull(avatars!, nameof(avatars));

            var data = new
            {
                character_ids = avatars.Select(x => x.Id),
                role_id = uid,
                server = server,
            };
            return await requester.PostWhileUpdateDynamicSecret2Async<DetailedAvatarWrapper>(
                $@"{ApiTakumiRecord}/character", data, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}