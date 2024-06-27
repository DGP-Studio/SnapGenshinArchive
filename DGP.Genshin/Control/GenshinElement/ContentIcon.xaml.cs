using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DGP.Genshin.Control.GenshinElement
{
    /// <summary>
    /// 角色武器图标
    /// </summary>
    public sealed partial class ContentIcon : Button
    {
        private const string FadeInAnimationKey = "FadeInAnimation";

        private static readonly DependencyProperty BackgroundUrlProperty = Property<ContentIcon>.Depend<string>(nameof(BackgroundUrl));
        private static readonly DependencyProperty ForegroundUrlProperty = Property<ContentIcon>.Depend<string>(nameof(ForegroundUrl));
        private static readonly DependencyProperty BadgeUrlProperty = Property<ContentIcon>.Depend<string>(nameof(BadgeUrl));
        private static readonly DependencyProperty IsCountVisibleProperty = Property<ContentIcon>.Depend(nameof(IsCountVisible), false);
        private static readonly DependencyProperty ForegroundOpacityProperty = Property<ContentIcon>.Depend(nameof(ForegroundOpacity), 1D);

        /// <summary>
        /// 构造一个新的图标
        /// </summary>
        public ContentIcon()
        {
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;
            Loaded += ContentIconLoaded;
            InitializeComponent();
        }

        /// <summary>
        /// 背景Url
        /// </summary>
        public string BackgroundUrl
        {
            get => (string)GetValue(BackgroundUrlProperty);

            set => SetValue(BackgroundUrlProperty, value);
        }

        /// <summary>
        /// 前景Url
        /// </summary>
        public string ForegroundUrl
        {
            get => (string)GetValue(ForegroundUrlProperty);

            set => SetValue(ForegroundUrlProperty, value);
        }

        /// <summary>
        /// 角标Url
        /// </summary>
        public string BadgeUrl
        {
            get => (string)GetValue(BadgeUrlProperty);

            set => SetValue(BadgeUrlProperty, value);
        }

        /// <summary>
        /// 数量是否可见
        /// </summary>
        public bool IsCountVisible
        {
            get => (bool)GetValue(IsCountVisibleProperty);

            set => SetValue(IsCountVisibleProperty, value);
        }

        /// <summary>
        /// 前景透明度
        /// </summary>
        public double ForegroundOpacity
        {
            get => (double)GetValue(ForegroundOpacityProperty);

            set => SetValue(ForegroundOpacityProperty, value);
        }

        private void ContentIconLoaded(object sender, RoutedEventArgs e)
        {
            (FindResource(FadeInAnimationKey) as Storyboard)?.Begin();

            // thus only affect first load
            Loaded -= ContentIconLoaded;
        }
    }
}