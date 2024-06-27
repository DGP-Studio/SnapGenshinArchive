using DGP.Genshin.MiHoYoAPI.Request;
using DGP.Genshin.MiHoYoAPI.Response;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI
{
    /// <summary>
    /// 米哈游蓝贴服务器状态检测
    /// </summary>
    public class Status
    {
        private const string Hk4eApi = @"https://hk4e-api.mihoyo.com";

        /// <summary>
        /// 检测服务是否可用
        /// </summary>
        /// <returns>可用性</returns>
        public async Task<bool> CheckStatusAsync()
        {
            // Credit @HolographicHat
            Response<StatusData>? response = await new Requester().GetAsync<StatusData>($"{Hk4eApi}/event/bluepost/ping");
            return response != null && response.Data != null;
        }

        /// <summary>
        /// 时间
        /// </summary>
        private class StatusData
        {
            /// <summary>
            /// 当前时间
            /// </summary>
            [JsonProperty("now_time")]
            public long NowTime { get; set; }
        }
    }
}