using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Announcement
{
    /// <summary>
    /// 公告
    /// </summary>
    public class AnnouncementProvider
    {
        private const string Hk4eApi = "https://hk4e-api.mihoyo.com";
        private const string Query = "game=hk4e&game_biz=hk4e_cn&lang=zh-cn&bundle_id=hk4e_cn&platform=pc&region=cn_gf01&level=55&uid=100000000";

        /// <summary>
        /// 异步获取公告列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>公告列表</returns>
        public async Task<AnnouncementWrapper?> GetAnnouncementWrapperAsync(CancellationToken cancellationToken = default)
        {
            Response<AnnouncementWrapper>? resp =
                await new Requester().GetAsync<AnnouncementWrapper>(
                    $"{Hk4eApi}/common/hk4e_cn/announcement/api/getAnnList?{Query}",
                    cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }

        /// <summary>
        /// 异步获取公告内容列表
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>公告内容列表</returns>
        public async Task<List<AnnouncementContent>> GetAnnouncementContentsAsync(CancellationToken cancellationToken = default)
        {
            Requester requester = new(
                new()
                {
                    { "Accept", "*/*" },
                    { "Accept-Encoding", "gzip" },
                },
                useGZipCompression: true);
            Response<ListWrapper<AnnouncementContent>>? resp =
                await requester.GetAsync<ListWrapper<AnnouncementContent>>(
                    $"{Hk4eApi}/common/hk4e_cn/announcement/api/getAnnContent?{Query}",
                    cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data?.List ?? new();
        }
    }
}