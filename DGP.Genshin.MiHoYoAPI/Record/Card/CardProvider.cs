using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Request.DynamicSecret;
using DGP.Genshin.MiHoYoAPI.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Record.Card
{
    public class CardProvider
    {
        private const string BBSApi = @"https://bbs-api.mihoyo.com";
        private const string Referer = "https://webstatic.mihoyo.com/app/community-game-records/index.html?bbs_presentation_style=fullscreen";
        private readonly string cookie;

        /// <summary>
        /// 初始化 <see cref="CardProvider"/> 的实例
        /// </summary>
        /// <param name="cookie"></param>
        public CardProvider(string cookie)
        {
            this.cookie = cookie;
        }

        /// <summary>
        /// 获取游戏展示卡片信息
        /// 提供的cookie需要包含 stuid 与 stoken
        /// </summary>
        /// <param name="uid">米游社uid，可以是别人的uid</param>
        /// <returns></returns>
        public async Task<List<Card>> GetGameRecordCardAsync(string uid, CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"x-rpc-app_version", DynamicSecretProvider2.AppVersion },
                {"User-Agent",RequestOptions.CommonUA2352 },
                {"x_rpc_client_type", "5" },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            ListWrapper<Card>? resp = await requester.GetWhileUpdateDynamicSecret2Async<ListWrapper<Card>>(
                $"{BBSApi}/game_record/app/card/wapi/getGameRecordCard?uid={uid}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.List ?? new();
        }
    }
}
