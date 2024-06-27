using DGP.Genshin.MiHoYoAPI.Calculation.Filter;
using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Response;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    /// <summary>
    /// 养成计算器
    /// </summary>
    public class Calculator
    {
        private const string ApiTakumi = @"https://api-takumi.mihoyo.com";
        private const string ReferBaseUrl = @"https://webstatic.mihoyo.com/ys/event/e20200923adopt_calculator/index.html";

        private static readonly string Referer =
            $"{ReferBaseUrl}?bbs_presentation_style=fullscreen&bbs_auth_required=true&mys_source=GameRecord";

        private readonly string cookie;

        public Calculator(string cookie)
        {
            this.cookie = cookie;
        }

        /// <summary>
        /// 获取筛选器
        /// </summary>
        /// <param name="avatarId">角色id</param>
        /// <param name="uid">游戏内uid</param>
        /// <param name="region">服务器名称</param>
        /// <returns></returns>
        public async Task<Filters?> GetFiltersAsync(CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<Filters>? resp = await requester.GetAsync<Filters>
                ($"{ApiTakumi}/event/e20200928calculate/v1/item/filter", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }

        /// <summary>
        /// 获取所有可计算的角色
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="isRandomDelayEnabled"></param>
        /// <returns></returns>
        public async Task<List<Avatar>> GetAvatarListAsync(AllAvatarIdFilter filter, bool isRandomDelayEnabled = false, CancellationToken cancellationToken = default)
        {
            int currentPage = 1;
            Random random = new();
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            List<Avatar> avatars = new();
            Response<ListWrapper<Avatar>>? resp;
            do
            {
                filter.Page = currentPage++;
                resp = await requester.PostAsync<ListWrapper<Avatar>>
                    ($"{ApiTakumi}/event/e20200928calculate/v1/avatar/list", filter, cancellationToken)
                    .ConfigureAwait(false);
                //add to cached list
                if (resp?.Data?.List is not null)
                {
                    avatars.AddRange(resp.Data.List);
                }

                if (currentPage != 1 && isRandomDelayEnabled)
                {
                    await Task.Delay(random.Next(0, 1000), cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            while (resp?.Data?.List?.Count == 20);

            return avatars;
        }

        /// <summary>
        /// 获取未拥有的角色的技能列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Skill>> GetAvatarSkillListAsync(Avatar avatar, CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<ListWrapper<Skill>>? resp = await requester.GetAsync<ListWrapper<Skill>>
                ($"{ApiTakumi}/event/e20200928calculate/v1/avatarSkill/list?avatar_id={avatar.Id}&element_attr_id={avatar.ElementAttrId}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data?.List ?? new();
        }

        /// <summary>
        /// 获取未拥有的武器列表
        /// </summary>
        /// <param name="avatarId">角色id</param>
        /// <returns></returns>
        public async Task<List<Weapon>> GetWeaponListAsync(WeaponIdFilter filter, bool isRandomDelayEnabled = false, CancellationToken cancellationToken = default)
        {
            int currentPage = 1;
            Random random = new();
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            List<Weapon> weapons = new();
            Response<ListWrapper<Weapon>>? resp;
            do
            {
                filter.Page = currentPage++;
                resp = await requester.PostAsync<ListWrapper<Weapon>>
                    ($"{ApiTakumi}/event/e20200928calculate/v1/weapon/list", filter, cancellationToken)
                    .ConfigureAwait(false);
                //add to cached list
                if (resp?.Data?.List is not null)
                {
                    weapons.AddRange(resp.Data.List);
                }

                if (currentPage != 1 && isRandomDelayEnabled)
                {
                    await Task.Delay(random.Next(0, 1000), cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            while (resp?.Data?.List?.Count == 20);

            return weapons;
        }

        /// <summary>
        /// 获取未拥有的圣遗物列表
        /// </summary>
        /// <param name="avatarId">角色id</param>
        /// <returns></returns>
        public async Task<List<Reliquary>> GetReliquaryListAsync(ReliquaryIdFilter filter, bool isRandomDelayEnabled = false, CancellationToken cancellationToken = default)
        {
            int currentPage = 1;
            Random random = new();
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            List<Reliquary> reliquaries = new();
            Response<ListWrapper<Reliquary>>? resp;
            do
            {
                filter.Page = currentPage++;
                resp = await requester.PostAsync<ListWrapper<Reliquary>>
                    ($"{ApiTakumi}/event/e20200928calculate/v1/reliquary/list", filter, cancellationToken)
                    .ConfigureAwait(false);
                //add to cached list
                if (resp?.Data?.List is not null)
                {
                    reliquaries.AddRange(resp.Data.List);
                }

                if (currentPage != 1 && isRandomDelayEnabled)
                {
                    await Task.Delay(random.Next(0, 1000), cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            while (resp?.Data?.List?.Count == 20);

            return reliquaries;
        }

        /// <summary>
        /// 获取圣遗物匹配套装
        /// </summary>
        /// <param name="avatarId">角色id</param>
        /// <param name="uid">游戏内uid</param>
        /// <param name="region">服务器名称</param>
        /// <returns></returns>
        public async Task<List<Reliquary>> GetReliquarySetAsync(int reliquaryId, CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<ReliquaryListWrapper>? resp = await requester.GetAsync<ReliquaryListWrapper>
                ($"{ApiTakumi}/event/e20200928calculate/v1/reliquary/set?reliquary_id={reliquaryId}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data?.ReliquaryList ?? new();
        }

        #region sync
        /// <summary>
        /// 同步当前cookie的角色
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="isRandomDelayEnabled"></param>
        /// <returns></returns>
        public async Task<List<Avatar>> GetSyncedAvatarListAsync(SyncAvatarIdFilter filter, bool isRandomDelayEnabled = false, CancellationToken cancellationToken = default)
        {
            int currentPage = 1;
            Random random = new();
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            List<Avatar> avatars = new();
            Response<ListWrapper<Avatar>>? resp;
            do
            {
                filter.Page = currentPage++;
                resp = await requester.PostAsync<ListWrapper<Avatar>>
                    ($"{ApiTakumi}/event/e20200928calculate/v1/sync/avatar/list", filter, cancellationToken)
                    .ConfigureAwait(false);
                //add to cached list
                if (resp?.Data?.List is not null)
                {
                    avatars.AddRange(resp.Data.List);
                }

                if (currentPage != 1 && isRandomDelayEnabled)
                {
                    await Task.Delay(random.Next(0, 1000), cancellationToken);
                }
            }
            while (resp?.Data?.List?.Count == 20);

            return avatars;
        }

        /// <summary>
        /// 获取角色详细计算信息
        /// </summary>
        /// <param name="avatarId">角色id</param>
        /// <param name="uid">游戏内uid</param>
        /// <param name="region">服务器名称</param>
        /// <returns></returns>
        public async Task<AvatarDetailData?> GetSyncedAvatarDetailDataAsync(int avatarId, string uid, string region, CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<AvatarDetailData>? resp = await requester.GetAsync<AvatarDetailData>
                ($"{ApiTakumi}/event/e20200928calculate/v1/sync/avatar/detail?avatar_id={avatarId}&uid={uid}&region={region}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }
        #endregion

        /// <summary>
        /// 计算所需的材料
        /// </summary>
        /// <param name="promotion">提升的增量</param>
        /// <returns>所需的材料</returns>
        public async Task<Consumption?> ComputeAsync(AvatarPromotionDelta promotion, CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<Consumption>? resp = await requester.PostAsync<Consumption>
                ($"{ApiTakumi}/event/e20200928calculate/v2/compute", promotion, cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }
    }
}