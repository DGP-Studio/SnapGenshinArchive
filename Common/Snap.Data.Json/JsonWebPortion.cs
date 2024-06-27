using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Snap.Data.Json
{
    /// <summary>
    /// Json 操作的联网部分
    /// </summary>
    public static partial class Json
    {
        private static readonly Lazy<HttpClient> LazyHttpClient = new(() =>
        {
            HttpClient client = new()
            {
                Timeout = Timeout.InfiniteTimeSpan,
            };
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Snap.Json");
            return client;
        });

        /// <summary>
        /// 从网站上下载json并转换为对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="url">链接</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>Json字符串中的反序列化对象, 如果反序列化失败会抛出异常</returns>
        public static async Task<T?> FromWebsiteAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            string response = await LazyHttpClient.Value.GetStringAsync(url, cancellationToken);
            return ToObject<T>(response);
        }

        /// <summary>
        /// 将网页内容保存到本地
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        public static async Task WebsiteToFileAsync(string url, string filePath, CancellationToken cancellationToken = default)
        {
            string response = await LazyHttpClient.Value.GetStringAsync(url, cancellationToken);
            await File.WriteAllTextAsync(filePath, response, cancellationToken);
        }
    }
}