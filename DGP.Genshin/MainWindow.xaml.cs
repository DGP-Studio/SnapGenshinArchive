using DGP.Genshin.Core.Background;
using DGP.Genshin.Core.DpiAware;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.Setting;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.Threading;
using Snap.Threading;
using System.ComponentModel;
using System.Threading;

namespace DGP.Genshin
{
    /// <summary>
    /// 主窗体
    /// </summary>
    internal partial class MainWindow : Window
    {
        private const int MinSaveableWidth = 600;
        private const int MinSaveableHeight = 375;

        private static bool hasEverOpen = false;
        private static bool hasEverClose = false;

        // make sure while post-initializing, main window can't be closed
        // prevent System.NullReferenceException
        // cause we have some async operation in initialization so we can't use lock
        private readonly SemaphoreSlim initializingWindow = new(1, 1);
        private readonly INavigationService navigationService;
        private readonly BackgroundLoader backgroundLoader;
        private readonly MainRecipient mainRecipient;

        /// <summary>
        /// 构造新的主窗体的实例
        /// Do NOT set DataContext for mainwindow
        /// </summary>
        public MainWindow()
        {
            navigationService = App.AutoWired<INavigationService>();
            mainRecipient = new(this, navigationService);

            // suppress IDE0052
            _ = mainRecipient;

            InitializeContent();

            // support per monitor dpi awareness
            _ = new DpiAwareAdapter(this);

            // randomly load a image as background
            backgroundLoader = new(this, App.Messenger);
            backgroundLoader.LoadNextWallpaperAsync().Forget();

            // initialize NavigationService
            navigationService.NavigationView = NavView;
            navigationService.Frame = ContentFrame;
        }

        /// <summary>
        /// 是否曾打开过
        /// </summary>
        public static bool HasEverOpen { get => hasEverOpen; set => hasEverOpen = value; }

        /// <summary>
        /// 指示主窗体是否在初始化
        /// </summary>
        public SemaphoreSlim InitializingWindow { get => initializingWindow; }

        /// <inheritdoc/>
        protected override void OnClosing(CancelEventArgs e)
        {
            navigationService.HasEverNavigated = false;
            UninitializeContent();
            if (InitializingWindow.CurrentCount < 1)
            {
                e.Cancel = true;
                return;
            }

            using (InitializingWindow.Enter())
            {
                base.OnClosing(e);
            }

            bool isTaskbarIconEnabled = Setting2.IsTaskBarIconEnabled.Get() && (App.Current.NotifyIcon is not null);

            if (isTaskbarIconEnabled)
            {
                if ((!hasEverClose) && Setting2.IsTaskBarIconHintDisplay.Get())
                {
                    new ToastContentBuilder()
                    .AddText("Snap Genshin 已转入后台运行")
                    .AddText("点击托盘图标以显示主窗口")
                    .AddButton(new ToastButton()
                        .SetContent("不再显示")
                        .AddArgument("taskbarhint", "hide")
                        .SetBackgroundActivation())
                    .SafeShow(false);
                    hasEverClose = true;
                }
            }
            else
            {
                App.Current.Shutdown();
            }
        }

        private void InitializeContent()
        {
            InitializeComponent();

            // restore width and height from setting
            Width = Setting2.MainWindowWidth;
            Height = Setting2.MainWindowHeight;

            // restore pane state
            NavView.IsPaneOpen = Setting2.IsNavigationViewPaneOpen;
        }

        private void UninitializeContent()
        {
            if (WindowState == WindowState.Normal)
            {
                Setting2.MainWindowWidth.Set(Width < MinSaveableWidth ? MinSaveableWidth : Width);
                Setting2.MainWindowHeight.Set(Height < MinSaveableHeight ? MinSaveableHeight : Height);
            }

            Setting2.IsNavigationViewPaneOpen.Set(NavView.IsPaneOpen);
        }
    }
}