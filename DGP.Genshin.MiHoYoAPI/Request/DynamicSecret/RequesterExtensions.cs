using DGP.Genshin.MiHoYoAPI.Response;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Request.DynamicSecret
{
    public static class RequesterExtensions
    {
        /// <summary>
        /// 使用2代动态密钥需要调用此扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requester"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<T?> GetWhileUpdateDynamicSecret2Async<T>(this Requester requester, string url, CancellationToken cancellationToken = default) where T : class
        {
            requester.Headers["DS"] = DynamicSecretProvider2.Create(url);
            Response<T>? resp = await requester.GetAsync<T>(url, cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }

        /// <summary>
        /// 使用2代动态密钥需要调用此扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requester"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<T?> PostWhileUpdateDynamicSecret2Async<T>(this Requester requester, string url, object data, CancellationToken cancellationToken = default) where T : class
        {
            requester.Headers["DS"] = DynamicSecretProvider2.Create(url, data);
            Response<T>? resp = await requester.PostAsync<T>(url, data, cancellationToken)
                .ConfigureAwait(false);
            return resp?.Data;
        }
    }
}
