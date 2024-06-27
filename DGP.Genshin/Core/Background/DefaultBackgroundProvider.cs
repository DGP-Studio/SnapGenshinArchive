using DGP.Genshin.Core.Background.Abstraction;
using DGP.Genshin.Core.ImplementationSwitching;
using Snap.Core.Logging;
using Snap.Data.Utility.Extension;
using Snap.Extenion.Enumerable;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DGP.Genshin.Core.Background
{
    /// <summary>
    /// 默认的背景图片提供器
    /// </summary>
    [SwitchableImplementation(typeof(IBackgroundProvider))]
    internal class DefaultBackgroundProvider : IBackgroundProvider
    {
        private const string BackgroundFolder = "Background";

        private static readonly List<string> SupportedFiles;
        private static readonly IEnumerable<string> SupportedExtensions = new List<string>()
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".bmp",
        };

        private static string? latestFilePath;

        static DefaultBackgroundProvider()
        {
            string folder = PathContext.Locate(BackgroundFolder);
            Directory.CreateDirectory(folder);
            SupportedFiles = Directory.EnumerateFiles(folder)
                .Where(path => SupportedExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()))
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<BitmapImage?> GetNextBitmapImageAsync()
        {
            await Task.Yield();
            if (SupportedFiles.GetRandomNoRepeat(latestFilePath) is string randomPath)
            {
                latestFilePath = randomPath;
                this.Log($"Loading background wallpaper from {randomPath}");
                return GetBitmapImageFromPath(randomPath);
            }

            return null;
        }

        private BitmapImage GetBitmapImageFromPath(string randomPath)
        {
            BitmapImage image = new();
            try
            {
                using (FileStream stream = new(randomPath, FileMode.Open))
                {
                    using (image.AsDisposableInit())
                    {
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                    }
                }
            }
            catch
            {
                Verify.FailOperation($"无法读取图片：{randomPath}");
            }

            return image;
        }
    }
}