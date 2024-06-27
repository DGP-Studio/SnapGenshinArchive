using DGP.Genshin.MiHoYoAPI.GameRole;
using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Request.DynamicSecret;
using DGP.Genshin.MiHoYoAPI.Response;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Sign
{
    public class SignInProvider
    {
        private const string ApiTakumi = @"https://api-takumi.mihoyo.com";
        private const string ReferBaseUrl = @"https://webstatic.mihoyo.com/bbs/event/signin-ys/index.html";
        private const string ActivityId = "e202009291139501";

        private static readonly string Referer =
            $"{ReferBaseUrl}?bbs_auth_required=true&act_id={ActivityId}&utm_source=bbs&utm_medium=mys&utm_campaign=icon";

        private readonly string cookie;

        /// <summary>
        /// 初始化 <see cref="SignInProvider"/> 的实例
        /// </summary>
        /// <param name="cookie"></param>
        public SignInProvider(string cookie)
        {
            this.cookie = cookie;
        }

        /// <summary>
        /// 签到通用请求器
        /// </summary>
        private Requester SignInRequester
        {
            get
            {
                return new(new RequestOptions
                {
                    {"DS", DynamicSecretProvider.Create() },
                    {"x-rpc-app_version", DynamicSecretProvider.AppVersion },
                    {"User-Agent", RequestOptions.CommonUA2352 },
                    {"x-rpc-device_id", RequestOptions.DeviceId },
                    {"Accept", RequestOptions.Json },
                    {"x-rpc-client_type", "5" },
                    {"Referer", Referer },
                    {"Cookie", cookie },
                    {"X-Requested-With", RequestOptions.Hyperion },
                });
            }
        }

        /// <summary>
        /// 获取当前签到信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<SignInInfo?> GetSignInInfoAsync(UserGameRole role, CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent",RequestOptions.CommonUA2101 },
                {"x-rpc-device_id", RequestOptions.DeviceId },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            string query = $"act_id={ActivityId}&region={role.Region}&uid={role.GameUid}";
            Response<SignInInfo>? resp = await requester
                .GetAsync<SignInInfo>($"{ApiTakumi}/event/bbs_sign_reward/info?{query}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }

        /// <summary>
        /// 获取当前补签信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ReSignInfo?> GetReSignInfoAsync(UserGameRole role, CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent",RequestOptions.CommonUA2352 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            string query = $"act_id={ActivityId}&region={role.Region}&uid={role.GameUid}";
            Response<ReSignInfo>? resp = await requester
                .GetAsync<ReSignInfo>($"{ApiTakumi}/event/bbs_sign_reward/resign_info?{query}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<(bool IsOk, string Result)> SignInAsync(UserGameRole role, CancellationToken cancellationToken = default)
        {
            var data = new { act_id = ActivityId, region = role.Region, uid = role.GameUid };
            Requester requester = SignInRequester;
            Response<SignInResult>? resp = await requester
                .PostAsync<SignInResult>($"{ApiTakumi}/event/bbs_sign_reward/sign", data, cancellationToken)
                .ConfigureAwait(false);

            if (resp == null)
            {
                return (false, "请求失败");
            }
            else
            {
                if (resp.Data == null)
                {
                    return resp.ReturnCode switch
                    {
                        0 => (true, "签到成功"),
                        -100 or -5003 => (true, resp.Message!),
                        _ => (false, resp.Message!),
                    };
                }
                else
                {
                    if (resp.Data.RiskCode == 0 && resp.Data.Success == 0)
                    {
                        return (true, "签到成功");
                    }
                    else
                    {
                        return (false, $"[RiskCode: {resp.Data.RiskCode} | Success: {resp.Data.Success}] 账号受到风控!");
                    }
                }
            }
        }

        /// <summary>
        /// 补签
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<string> ReSignAsync(UserGameRole role, CancellationToken cancellationToken = default)
        {
            var data = new { act_id = ActivityId, region = role.Region, uid = role.GameUid };
            Requester requester = SignInRequester;
            Response<SignInResult>? resp = await requester
                .PostAsync<SignInResult>($"{ApiTakumi}/event/bbs_sign_reward/resign", data, cancellationToken)
                .ConfigureAwait(false);
            return resp is null ? "签到失败" : resp.Message ?? "签到成功";
        }

        /// <summary>
        /// 获取签到奖励
        /// </summary>
        /// <returns></returns>
        public async Task<SignInReward?> GetSignInRewardAsync(CancellationToken cancellationToken = default)
        {
            Requester requester = new(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer", Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<SignInReward>? resp = await requester
                .GetAsync<SignInReward>($"{ApiTakumi}/event/bbs_sign_reward/home?act_id={ActivityId}", cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }
    }
}
