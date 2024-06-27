using Microsoft.VisualStudio.Threading;
using Snap.Data.Utility.Extension;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DGP.Genshin.Control.Infrastructure.CachedImage
{
    /// <summary>
    /// Represents an icon that uses a bitmap as its content.
    /// </summary>
    [SuppressMessage("", "SA1124")]
    [SuppressMessage("", "SA1201")]
    [SuppressMessage("", "SA1202")]
    [SuppressMessage("", "SA1309")]
    [SuppressMessage("", "SA1413")]
    [SuppressMessage("", "SA1600")]
    public sealed class CachedBitmapIcon : CachedIconElementBase
    {
        static CachedBitmapIcon()
        {
            ForegroundProperty.OverrideMetadata(typeof(CachedBitmapIcon), new FrameworkPropertyMetadata(OnForegroundChanged));
        }

        /// <summary>
        /// Initializes a new instance of the CachedBitmapIcon class.
        /// </summary>
        public CachedBitmapIcon()
        {
        }

        #region UriSource

        /// <summary>
        /// Identifies the UriSource dependency property.
        /// </summary>
        public static readonly DependencyProperty UriSourceProperty =
            BitmapImage.UriSourceProperty.AddOwner(
                typeof(CachedBitmapIcon),
                new FrameworkPropertyMetadata(OnUriSourceChanged));

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of the bitmap to use as the
        /// icon content.
        /// </summary>
        /// <returns>The Uri of the bitmap to use as the icon content. The default is **null**.</returns>
        public Uri UriSource
        {
            get => (Uri)GetValue(UriSourceProperty);

            set => SetValue(UriSourceProperty, value);
        }

        private static void OnUriSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CachedBitmapIcon)d).ApplyUriSource();
        }

        #endregion

        #region ShowAsMonochrome

        /// <summary>
        /// Identifies the ShowAsMonochrome dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowAsMonochromeProperty =
            DependencyProperty.Register(
                nameof(ShowAsMonochrome),
                typeof(bool),
                typeof(CachedBitmapIcon),
                new PropertyMetadata(true, OnShowAsMonochromeChanged));

        /// <summary>
        /// Gets or sets a value that indicates whether the bitmap is shown in a single color.
        /// </summary>
        /// <returns>
        /// **true** to show the bitmap in a single color; **false** to show the bitmap in
        /// full color. The default is **true.**
        /// </returns>
        public bool ShowAsMonochrome
        {
            get => (bool)GetValue(ShowAsMonochromeProperty);

            set => SetValue(ShowAsMonochromeProperty, value);
        }

        private static void OnShowAsMonochromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CachedBitmapIcon)d).ApplyShowAsMonochrome();
        }

        #endregion

        private protected override void InitializeChildren()
        {
            _image = new CachedImage
            {
                Visibility = Visibility.Hidden,
                Stretch = Stretch.Uniform,
            };

            _opacityMask = new ImageBrush();
            _foreground = new Rectangle
            {
                OpacityMask = _opacityMask
            };

            ApplyForeground();
            ApplyUriSource();

            Children.Add(_image);

            ApplyShowAsMonochrome();
        }

        private protected override void OnShouldInheritForegroundFromVisualParentChanged()
        {
            ApplyForeground();
        }

        private protected override void OnVisualParentForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (ShouldInheritForegroundFromVisualParent)
            {
                ApplyForeground();
            }
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CachedBitmapIcon)d).ApplyForeground();
        }

        private void ApplyForeground()
        {
            if (_foreground != null)
            {
                _foreground.Fill = ShouldInheritForegroundFromVisualParent ? VisualParentForeground : Foreground;
            }
        }

        private void ApplyUriSource()
        {
            ApplyUriSourceAsync().Forget();
        }

        private async Task ApplyUriSourceAsync()
        {
            if (_image != null && _opacityMask != null)
            {
                Uri uriSource = UriSource;
                if (uriSource != null)
                {
                    MemoryStream? stream = await FileCache.HitAsync(uriSource.ToString());
                    if (stream is not null)
                    {
                        try
                        {
                            BitmapImage imageSource = new();
                            using (imageSource.AsDisposableInit())
                            {
                                // imageSource.CreateOptions = BitmapCreateOptions.None;
                                imageSource.StreamSource = stream;
                            }

                            _image.Source = imageSource;
                            _opacityMask.ImageSource = imageSource;
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    _image.ClearValue(Image.SourceProperty);
                    _opacityMask.ClearValue(ImageBrush.ImageSourceProperty);
                }
            }
        }

        private void ApplyShowAsMonochrome()
        {
            bool showAsMonochrome = ShowAsMonochrome;

            if (_image != null)
            {
                _image.Visibility = showAsMonochrome ? Visibility.Hidden : Visibility.Visible;
            }

            if (_foreground != null)
            {
                if (showAsMonochrome)
                {
                    if (!Children.Contains(_foreground))
                    {
                        Children.Add(_foreground);
                    }
                }
                else
                {
                    Children.Remove(_foreground);
                }
            }
        }

        private Image? _image;
        private Rectangle? _foreground;
        private ImageBrush? _opacityMask;
    }
}