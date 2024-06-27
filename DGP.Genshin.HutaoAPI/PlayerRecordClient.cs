using DGP.Genshin.HutaoAPI.GetModel;
using DGP.Genshin.HutaoAPI.PostModel;
using DGP.Genshin.MiHoYoAPI.GameRole;
using DGP.Genshin.MiHoYoAPI.Record;
using DGP.Genshin.MiHoYoAPI.Record.Avatar;
using DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss;
using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Response;
using Microsoft;
using Snap.Extenion.Enumerable;
using Snap.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.HutaoAPI
{
    /// <summary>
    /// 玩家记录客户端
    /// </summary>
    public class PlayerRecordClient
    {
        // auth.snapgenshin.com
        private const string HutaoAPIHost = "https://hutao-api.snapgenshin.com";
        private const string AuthAPIHost = "https://auth.snapgenshin.com";
        private const string ContentType = "text/json";

        private Requester AuthRequester { get; set; } = new();

        /// <summary>
        /// 异步登录获取token
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            // Please contract us for your own token
            Auth token = new("08d9e212-0cb3-4d71-8ed7-003606da7b20", "7ueWgZGn53dDhrm8L5ZRw+YWfOeSWtgQmJWquRgaygw=");
            Response<Token>? resp = await new Requester()
                .PostAsync<Token>($"{AuthAPIHost}/Auth/Login", token, ContentType, cancellationToken)
                .ConfigureAwait(false);

            Requires.NotNull(resp?.Data?.AccessToken!, nameof(resp.Data.AccessToken));
            AuthRequester = new() { UseAuthToken = true, AuthToken = resp.Data.AccessToken };
        }

        /// <summary>
        /// 异步获取所有记录并上传到数据库
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <param name="confirmAsyncFunc">异步确认委托</param>
        /// <param name="resultAsyncFunc">结果确认委托</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        [ExecuteOnMainThread]
        public async Task GetAllRecordsAndUploadAsync(string cookie, Func<PlayerRecord, Task<bool>> confirmAsyncFunc, Func<Response, Task> resultAsyncFunc, CancellationToken cancellationToken = default)
        {
            RecordProvider recordProvider = new(cookie);

            List<UserGameRole> userGameRoles =
                await new UserGameRoleProvider(cookie).GetUserGameRolesAsync(cancellationToken)
                .ConfigureAwait(true);

            foreach (UserGameRole role in userGameRoles)
            {
                Requires.NotNull(role.GameUid!, nameof(role.GameUid));
                Requires.NotNull(role.Region!, nameof(role.Region));

                PlayerInfo? playerInfo = await recordProvider
                    .GetPlayerInfoAsync(role.GameUid, role.Region, cancellationToken)
                    .ConfigureAwait(true);
                Requires.NotNull(playerInfo!, nameof(playerInfo));

                DetailedAvatarWrapper? detailAvatars = await recordProvider
                    .GetDetailAvaterInfoAsync(role.GameUid, role.Region, playerInfo, cancellationToken)
                    .ConfigureAwait(true);
                Requires.NotNull(detailAvatars!, nameof(detailAvatars));

                SpiralAbyss? spiralAbyssInfo = await recordProvider
                    .GetSpiralAbyssAsync(role.GameUid, role.Region, SpiralAbyssType.Current, cancellationToken)
                    .ConfigureAwait(true);
                Requires.NotNull(spiralAbyssInfo!, nameof(spiralAbyssInfo));

                PlayerRecord playerRecord = PlayerRecordBuilder.BuildPlayerRecord(role.GameUid, detailAvatars, spiralAbyssInfo);
                if (await confirmAsyncFunc.Invoke(playerRecord).ConfigureAwait(true))
                {
                    Response<string>? resp = null;
                    if (Response.IsOk(await UploadItemsAsync(detailAvatars, cancellationToken).ConfigureAwait(true)))
                    {
                        resp = await AuthRequester.PostAsync<string>($"{HutaoAPIHost}/Record/Upload", playerRecord, ContentType, cancellationToken)
                            .ConfigureAwait(true);
                    }

                    await resultAsyncFunc.Invoke(resp ?? Response.CreateFail($"{role.GameUid}-记录提交失败。"))
                        .ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// 异步获取总览数据
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>总览信息</returns>
        public async Task<Overview?> GetOverviewAsync(CancellationToken cancellationToken = default)
        {
            Response<Overview>? resp = await AuthRequester
                .GetAsync<Overview>($"{HutaoAPIHost}/Statistics/Overview", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }

        /// <summary>
        /// 异步获取角色出场率
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色出场率</returns>
        [Obsolete]
        public async Task<IEnumerable<AvatarParticipation>> GetAvatarParticipationsAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<AvatarParticipation>>? resp = await AuthRequester
                .GetAsync<IEnumerable<AvatarParticipation>>($"{HutaoAPIHost}/Statistics/AvatarParticipation", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<AvatarParticipation>();
        }

        /// <summary>
        /// 异步获取角色使用率
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色使用率</returns>
        public async Task<IEnumerable<AvatarParticipation>> GetAvatarParticipation2sAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<AvatarParticipation>>? resp = await AuthRequester
                .GetAsync<IEnumerable<AvatarParticipation>>($"{HutaoAPIHost}/Statistics2/AvatarParticipation", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<AvatarParticipation>();
        }

        /// <summary>
        /// 异步获取角色圣遗物搭配
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色圣遗物搭配</returns>
        public async Task<IEnumerable<AvatarReliquaryUsage>> GetAvatarReliquaryUsagesAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<AvatarReliquaryUsage>>? resp = await AuthRequester
                .GetAsync<IEnumerable<AvatarReliquaryUsage>>($"{HutaoAPIHost}/Statistics/AvatarReliquaryUsage", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<AvatarReliquaryUsage>();
        }

        /// <summary>
        /// 异步获取角色搭配数据
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色搭配数据</returns>
        public async Task<IEnumerable<TeamCollocation>> GetTeamCollocationsAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<TeamCollocation>>? resp = await AuthRequester
                .GetAsync<IEnumerable<TeamCollocation>>($"{HutaoAPIHost}/Statistics/TeamCollocation", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<TeamCollocation>();
        }

        /// <summary>
        /// 异步获取角色武器搭配数据
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色武器搭配数据</returns>
        public async Task<IEnumerable<WeaponUsage>> GetAvatarWeaponUsagesAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<WeaponUsage>>? resp = await AuthRequester
                .GetAsync<IEnumerable<WeaponUsage>>($"{HutaoAPIHost}/Statistics/AvatarWeaponUsage", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<WeaponUsage>();
        }

        /// <summary>
        /// 异步获取角色图片列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色图片列表</returns>
        public async Task<IEnumerable<HutaoItem>> GetAvatarMapAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<HutaoItem>>? resp = await AuthRequester
                .GetAsync<IEnumerable<HutaoItem>>($"{HutaoAPIHost}/GenshinItem/Avatars", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data?.DistinctBy(x => x.Id) ?? Enumerable.Empty<HutaoItem>();
        }

        /// <summary>
        /// 异步获取武器图片列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>武器图片列表</returns>
        public async Task<IEnumerable<HutaoItem>> GetWeaponMapAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<HutaoItem>>? resp = await AuthRequester
                .GetAsync<IEnumerable<HutaoItem>>($"{HutaoAPIHost}/GenshinItem/Weapons", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data?.DistinctBy(x => x.Id) ?? Enumerable.Empty<HutaoItem>();
        }

        /// <summary>
        /// 异步获取圣遗物图片列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>圣遗物图片列表</returns>
        public async Task<IEnumerable<HutaoItem>> GetReliquaryMapAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<HutaoItem>>? resp = await AuthRequester
                .GetAsync<IEnumerable<HutaoItem>>($"{HutaoAPIHost}/GenshinItem/Reliquaries", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data?.DistinctBy(x => x.Id) ?? Enumerable.Empty<HutaoItem>();
        }

        /// <summary>
        /// 异步获取角色命座信息
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>角色图片列表</returns>
        public async Task<IEnumerable<AvatarConstellationNum>> GetAvatarConstellationsAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<AvatarConstellationNum>>? resp = await AuthRequester
                .GetAsync<IEnumerable<AvatarConstellationNum>>($"{HutaoAPIHost}/Statistics/Constellation", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<AvatarConstellationNum>();
        }

        /// <summary>
        /// 异步获取队伍出场次数
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>队伍出场列表</returns>
        public async Task<IEnumerable<TeamCombination>> GetTeamCombinationsAsync(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<TeamCombination>>? resp = await AuthRequester
                .GetAsync<IEnumerable<TeamCombination>>($"{HutaoAPIHost}/Statistics/TeamCombination", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<TeamCombination>();
        }

        /// <summary>
        /// 异步获取队伍出场次数2
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>队伍出场列表</returns>
        public async Task<IEnumerable<TeamCombination2>> GetTeamCombinations2Async(CancellationToken cancellationToken = default)
        {
            Response<IEnumerable<TeamCombination2>>? resp = await AuthRequester
                .GetAsync<IEnumerable<TeamCombination2>>($"{HutaoAPIHost}/Statistics2/TeamCombination", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data ?? Enumerable.Empty<TeamCombination2>();
        }

        /// <summary>
        /// 检查对应的uid当前是否上传了数据
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>当前是否上传了数据</returns>
        public async Task<bool> CheckPeriodRecordUploadedAsync(string uid, CancellationToken cancellationToken = default)
        {
            Response<UploadInfo>? resp = await AuthRequester
                .GetAsync<UploadInfo>($"{HutaoAPIHost}/Record/CheckRecord/{uid}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data is not null && resp.Data.PeriodUploaded;
        }

        /// <summary>
        /// 异步获取排行信息
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>排行信息</returns>
        public async Task<RankWrapper?> GetRankAsync(string uid, CancellationToken cancellationToken = default)
        {
            Response<RankWrapper>? resp = await AuthRequester
               .GetAsync<RankWrapper>($"{HutaoAPIHost}/Record/Rank/{uid}", cancellationToken)
               .ConfigureAwait(false);
            return resp?.Data;
        }

        /// <summary>
        /// 异步上传物品所有物品
        /// </summary>
        /// <param name="detailedAvatar">角色详细信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应</returns>
        private async Task<Response<string>?> UploadItemsAsync(DetailedAvatarWrapper detailedAvatar, CancellationToken cancellationToken = default)
        {
            IEnumerable<HutaoItem>? avatars = detailedAvatar.Avatars?
                .Select(avatar => new HutaoItem(avatar.Id, avatar.Name, avatar.Icon))
                .DistinctBy(item => item.Id);
            IEnumerable<HutaoItem>? weapons = detailedAvatar.Avatars?
                .Select(avatar => avatar.Weapon)
                .NotNull()
                .Select(weapon => new HutaoItem(weapon.Id, weapon.Name, weapon.Icon))
                .DistinctBy(item => item.Id);
            IEnumerable<HutaoItem>? reliquaries = detailedAvatar.Avatars?
                .Select(avatars => avatars.Reliquaries)
                .SelectMany(reliquaries => reliquaries!)
                .Where(relic => relic.Position == 1)
                .Select(relic => new HutaoItem(relic.ReliquarySet!.Id, relic.ReliquarySet.Name, relic.Icon))
                .DistinctBy(item => item.Id);

            GenshinItemWrapper? data = new() { Avatars = avatars, Weapons = weapons, Reliquaries = reliquaries };

            return await AuthRequester
                        .PostAsync<string>($"{HutaoAPIHost}​/GenshinItem/Upload", data, ContentType, cancellationToken)
                        .ConfigureAwait(false);
        }

        private record Auth(string Appid, string Secret);

        private record Token(string? AccessToken);
    }
}