using Snap.Core.Logging;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Control.Infrastructure.CachedImage
{
    /// <summary>
    /// https://github.com/floydpink/CachedImage/blob/main/source/FileCache.cs
    /// </summary>
    internal static class FileCache
    {
        private const string CacheFolderName = "Cache";

        // Record whether a file is being written.
        private static readonly Dictionary<string, bool> IsWritingFile = new();

        // HttpClient is intended to be instantiated once per application, rather than per-use.
        private static readonly Lazy<HttpClient> LazyHttpClient = new(() => new() { Timeout = Timeout.InfiniteTimeSpan });

        /// <summary>
        /// Url命中
        /// </summary>
        /// <param name="url">下载链接</param>
        /// <returns>缓存或下载的图片</returns>
        public static async Task<MemoryStream?> HitAsync(string? url)
        {
            if (url is null)
            {
                return null;
            }

            PathContext.CreateFolderOrIgnore(CacheFolderName);

            Uri uri = new(url);
            string fileName = BuildFileName(uri);
            string localFile = PathContext.Locate(CacheFolderName, fileName);

            MemoryStream memoryStream = new();
            FileStream? fileStream = null;

            // 未写文件且文件存在 读取文件缓存并返回
            if (!IsWritingFile.ContainsKey(fileName) && File.Exists(localFile))
            {
                using (fileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                // check the image format valid.
                try
                {
                    _ = Image.FromStream(memoryStream);
                }
                catch
                {
                    return null;
                }

                return memoryStream;
            }

            try
            {
                Logger.LogStatic($"Download {uri} as {fileName}");
                HttpClient client = LazyHttpClient.Value;
                HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    if (!IsWritingFile.ContainsKey(fileName))
                    {
                        IsWritingFile[fileName] = true;
                        fileStream = new FileStream(localFile, FileMode.Create, FileAccess.Write);
                    }

                    await CopyToCacheAndMemoryAsync(responseStream, memoryStream, fileStream, fileName);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                Logger.LogStatic($"Download {uri} completed.");
                return memoryStream;
            }
            catch (Exception ex)
            {
                await memoryStream.DisposeAsync();
                if (fileStream is not null)
                {
                    await fileStream.DisposeAsync();
                }

                Logger.LogStatic(ex);
                File.Delete(localFile);
                Logger.LogStatic($"Caching {url} To {fileName} failed.File has deleted.");
                return null;
            }
        }

        /// <summary>
        /// 检查图片是否已经缓存
        /// </summary>
        /// <param name="url">下载链接</param>
        /// <returns>图片是否已经缓存</returns>
        public static bool Exists(string? url)
        {
            Must.NotNull(url!);

            PathContext.CreateFolderOrIgnore(CacheFolderName);

            Uri uri = new(url);
            string fileName = BuildFileName(uri);
            string localFile = $"{CacheFolderName}\\{fileName}";
            return PathContext.FileExists(localFile);
        }

        /// <summary>
        /// 构造缓存文件名称
        /// </summary>
        /// <param name="uri">下载链接</param>
        /// <returns>文件名称</returns>
        private static string BuildFileName(Uri uri)
        {
            StringBuilder fileNameBuilder = new();
            using (SHA1 sha1 = SHA1.Create())
            {
                string canonicalUrl = uri.ToString();
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(canonicalUrl));
                fileNameBuilder.Append(BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant());
                if (Path.HasExtension(canonicalUrl))
                {
                    fileNameBuilder.Append(Path.GetExtension(canonicalUrl).Split('?')[0]);
                }
            }

            return fileNameBuilder.ToString();
        }

        /// <summary>
        /// 复制到缓存与文件
        /// </summary>
        /// <param name="response">响应流</param>
        /// <param name="memory">写入内存流</param>
        /// <param name="file">写入文件流</param>
        /// <param name="fileName">写入的文件名</param>
        /// <returns>可追踪的任务</returns>
        private static async Task CopyToCacheAndMemoryAsync(Stream response, MemoryStream memory, FileStream? file, string fileName)
        {
            const int bufferSize = 128;
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            do
            {
                bytesRead = await response.ReadAsync(buffer.AsMemory(0, bufferSize));
                if (file is not null)
                {
                    await file.WriteAsync(buffer.AsMemory(0, bytesRead));
                }

                await memory.WriteAsync(buffer.AsMemory(0, bytesRead));
            }
            while (bytesRead > 0);

            if (file is not null)
            {
                await file.FlushAsync();
                await file.DisposeAsync();
                IsWritingFile.Remove(fileName);
            }
        }
    }
}