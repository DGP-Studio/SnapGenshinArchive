using Snap.Core.Logging;
using Snap.Data.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snap.Net.Afdian
{
    /// <summary>
    /// Afdian API 专用请求器
    /// </summary>
    internal class AfdianRequester
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use.
        private static readonly Lazy<HttpClient> LazyHttpClient = new(() => new() { Timeout = Timeout.InfiniteTimeSpan });
        public AfdianRequestOptions Headers { get; set; } = new AfdianRequestOptions();

        /// <summary>
        /// 构造一个新的 <see cref="AfdianRequester"/> 对象
        /// </summary>
        public AfdianRequester() { }

        private async Task<T?> Request<T>(Func<HttpClient, AfdianRequestInfo> requestFunc, CancellationToken cancellation = default)
        {
            AfdianRequestInfo? info = null;

            HttpClient client = LazyHttpClient.Value;
            client.DefaultRequestHeaders.Clear();
            foreach ((string name, string value) in Headers)
            {
                client.DefaultRequestHeaders.Add(name, value);
            }
            info = requestFunc(client);
            try
            {
                HttpResponseMessage response = await info.RequestAsyncFunc.Invoke();
                HttpContent content = response.Content;
                return Json.ToObject<T>(await content.ReadAsStringAsync(cancellation));
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// POST 操作
        /// </summary>
        /// <typeparam name="T">返回的类类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="data">要发送的.NET（匿名）对象</param>
        /// <returns>响应</returns>
        public async Task<T?> PostAsync<T>(string? url, object data, CancellationToken cancellationToken = default) where T : class
        {
            string dataString = Json.Stringify(data)/*.ReplaceLineEndings(string.Empty)*/;
            this.Log($"POST {url} with\n{dataString}");
            return url is null ? null : await Request<T>(client =>
            new AfdianRequestInfo("POST", url,
                () => client.PostAsync(url, new StringContent(dataString, Encoding.UTF8, "application/json"), cancellationToken)),
            cancellationToken);
        }
    }
}
