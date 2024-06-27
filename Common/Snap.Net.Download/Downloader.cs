using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Snap.Net.Download
{
    /// <summary>
    /// 单线程下载器
    /// </summary>
    public class Downloader
    {
        private const int BufferSize = 8192;

        private static readonly Lazy<HttpClient> LazyHttpClient = new(() => new() { Timeout = Timeout.InfiniteTimeSpan });

        private readonly Uri downloadUrl;
        private readonly string destinationFilePath;

        /// <summary>
        /// 构造一个新的单线程下载器
        /// </summary>
        /// <param name="uri">待下载的uri</param>
        /// <param name="destinationFilePath">保存的文件位置</param>
        public Downloader(Uri uri, string destinationFilePath)
        {
            downloadUrl = uri;
            this.destinationFilePath = destinationFilePath;
        }

        /// <summary>
        /// 异步的下载文件
        /// </summary>
        /// <param name="progress">进度</param>
        /// <returns>任务</returns>
        public async Task DownloadAsync(IProgress<DownloadInfomation> progress)
        {
            using (HttpResponseMessage response = await LazyHttpClient.Value.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
            {
                await DownloadFileFromHttpResponseMessageAsync(response, progress);
            }
        }

        private async Task DownloadFileFromHttpResponseMessageAsync(HttpResponseMessage response, IProgress<DownloadInfomation> progress)
        {
            response.EnsureSuccessStatusCode();

            long? totalBytes = response.Content.Headers.ContentLength;

            using (Stream contentStream = await response.Content.ReadAsStreamAsync())
            {
                await ProcessContentStreamAsync(contentStream, totalBytes, progress);
            }
        }

        private async Task ProcessContentStreamAsync(Stream contentStream, long? totalDownloadSize, IProgress<DownloadInfomation> progress)
        {
            long totalBytesRead = 0L;
            long readCount = 0L;
            byte[] buffer = new byte[BufferSize];
            bool isMoreToRead = true;

            using (FileStream fileStream = new(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, true))
            {
                do
                {
                    int bytesRead = await contentStream.ReadAsync(buffer);
                    if (bytesRead == 0)
                    {
                        isMoreToRead = false;
                        progress.Report(new(totalBytesRead, totalDownloadSize ?? 0));
                        continue;
                    }

                    await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                    totalBytesRead += bytesRead;
                    readCount += 1;

                    if (readCount % 8 == 0)
                    {
                        progress.Report(new(totalBytesRead, totalDownloadSize ?? 0));
                    }
                }
                while (isMoreToRead);
            }
        }
    }
}