using Microsoft.VisualStudio.Threading;
using Snap.Data.Utility.Extension;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DGP.Genshin.Control.Infrastructure.CachedImage
{
    /// <summary>
    /// <see cref="System.Windows.Controls.Image"/> 的带缓存包装
    /// 实现本地图像缓存
    /// </summary>
    /// https://github.com/floydpink/CachedImage/blob/main/source/Image.cs
    public class CachedImage : System.Windows.Controls.Image
    {
        private static readonly DependencyProperty ImageUrlProperty = Property<CachedImage>.Depend(nameof(ImageUrl), string.Empty, ImageUrlPropertyChanged);
        private static readonly DependencyProperty CreateOptionsProperty = Property<CachedImage>.Depend<BitmapCreateOptions>(nameof(CreateOptions));

        static CachedImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CachedImage), new FrameworkPropertyMetadata(typeof(CachedImage)));
        }

        /// <summary>
        /// 图像Url
        /// </summary>
        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);

            set => SetValue(ImageUrlProperty, value);
        }

        /// <summary>
        /// 创建选项
        /// </summary>
        public BitmapCreateOptions CreateOptions
        {
            get => (BitmapCreateOptions)GetValue(CreateOptionsProperty);

            set => SetValue(CreateOptionsProperty, value);
        }

        private static void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HitImageAsync((CachedImage)obj, e.NewValue as string).Forget();
        }

        private static async Task HitImageAsync(CachedImage cachedImage, string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            BitmapImage bitmapImage = new();

            try
            {
                using (MemoryStream? memoryStream = await FileCache.HitAsync(url))
                {
                    if (memoryStream == null)
                    {
                        cachedImage.Source = null;
                        return;
                    }

                    using (bitmapImage.AsDisposableInit())
                    {
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.CreateOptions = cachedImage.CreateOptions;
                        bitmapImage.StreamSource = memoryStream;
                    }

                    bitmapImage.Freeze();
                    cachedImage.Source = bitmapImage;
                }
            }
            catch
            {
            }
        }
    }
}