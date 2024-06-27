using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.GameRole
{
    public class UserGameRoleProvider
    {
        private const string ApiTakumi = @"https://api-takumi.mihoyo.com";

        private readonly string cookie;
        public UserGameRoleProvider(string cookie)
        {
            this.cookie = cookie;
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <returns>用户角色信息</returns>
        public async Task<List<UserGameRole>> GetUserGameRolesAsync(CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                //{"Accept", RequestOptions.Json },
                //{"User-Agent", RequestOptions.CommonUA2101 },
                {"Cookie", cookie },
                //{"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<UserGameRoleInfo>? resp = await requester
                .GetAsync<UserGameRoleInfo>($"{ApiTakumi}/binding/api/getUserGameRolesByCookie?game_biz=hk4e_cn", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data?.List ?? new();
        }
    }
}
