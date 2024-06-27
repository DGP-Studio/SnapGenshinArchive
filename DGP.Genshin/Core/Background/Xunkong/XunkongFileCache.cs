using Snap.Core.Logging;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Core.Background.Xunkong
{
    /// <summary>
    /// https://github.com/floydpink/CachedImage/blob/main/source/FileCache.cs
    /// </summary>
    internal static class XunkongFileCache
    {
        private const string CacheFolderName = "Background/Xunkong";

        // Record whether a file is being written.
        private static readonly Dictionary<string, bool> IsWritingFile = new();
        private static readonly Lazy<HttpClient> LazyHttpClient = new(() => new() { Timeout = Timeout.InfiniteTimeSpan });

        /// <summary>
        /// Url命中
        /// </summary>
        /// <param name="url">图片Url</param>
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