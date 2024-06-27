using DGP.Genshin.Core.Background.Abstraction;
using DGP.Genshin.Core.ImplementationSwitching;
using Snap.Data.Json;
using Snap.Data.Utility.Extension;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DGP.Genshin.Core.Background.Xunkong
{
    /// <summary>
    /// 寻空壁纸API实现
    /// </summary>
    [SwitchableImplementation(typeof(IBackgroundProvider), "Xunkong.Wallpaper", "Xunkong 壁纸实现")]
    internal class XunkongWallpaperProvider : IBackgroundProvider
    {
        private const string Api = "https://api.xunkong.cc/v0.1/wallpaper/random";

        /// <inheritdoc/>
        public async Task<BitmapImage?> GetNextBitmapImageAsync()
        {
            try
            {
                XunkongResponse<XunkongWallpaperInfo>? result = await Json.FromWebsiteAsync<XunkongResponse<XunkongWallpaperInfo>>(Api);
                if (await XunkongFileCache.HitAsync(result?.Data?.Url) is MemoryStream stream)
                {
                    BitmapImage bitmapImage = new();
                    using (bitmapImage.AsDisposableInit())
                    {
                        bitmapImage.StreamSource = stream;
                    }

                    return bitmapImage;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}