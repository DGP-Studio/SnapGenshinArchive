using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.Core.Plugins;
using DGP.Genshin.Message.Internal;
using DGP.Genshin.Page;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.Setting;
using DGP.Genshin.Service.Abstraction.Updating;
using DGP.Genshin.ViewModel;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.Threading;
using Snap.Extenion.Enumerable;
using Snap.Reflection;
using Snap.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin
{
    /// <summary>
    /// 主窗体接收器
    /// 负责主窗体的各类初始化任务
    /// </summary>
    internal class MainRecipient : IRecipient<SplashInitializationCompletedMessage>
    {
        private static readonly TaskPreventer PostInitializationTaskPreventer = new();

        private readonly MainWindow mainWindow;
        private readonly INavigationService navigationService;

        /// <summary>
        /// 构造一个新的主窗体接收器
        /// </summary>
        /// <param name="mainWindow">主窗体</param>
        /// <param name="navigationService">导航服务</param>
        public MainRecipient(MainWindow mainWindow, INavigationService navigationService)
        {
            this.mainWindow = mainWindow;
            this.navigationService = navigationService;

            // register messages
            App.Messenger.Register(this);
        }

        /// <summary>
        /// 析构器
        /// </summary>
        ~MainRecipient()
        {
            App.Messenger.Unregister<SplashInitializationCompletedMessage>(this);
        }

        /// <inheritdoc/>
        public void Receive(SplashInitializationCompletedMessage viewModelReference)
        {
            PostInitializeAsync(viewModelReference).Forget();
        }

        private async Task PostInitializeAsync(SplashInitializationCompletedMessage viewModelReference)
        {
            if (PostInitializationTaskPreventer.ShouldExecute)
            {
                using (await mainWindow.InitializingWindow.EnterAsync())
                {
                    SplashViewModel splashViewModel = viewModelReference.Value;
                    AddAdditionalNavigationViewItems();

                    // preprocess
                    if (!MainWindow.HasEverOpen)
                    {
                        Setting2.AppVersion.Set(App.AutoWired<IUpdateService>().CurrentVersion);

                        CheckUpdateForNotificationAsync().Forget();
                        TrySignInOnStartUpAsync().Forget();

                        TryInitializeTaskbarIcon();

                        // 树脂服务
                        App.AutoWired<IDailyNoteService>().Initialize();
                    }

                    splashViewModel.CompleteInitialization();
                }

                // 首次启动
                if (!MainWindow.HasEverOpen)
                {
                    // 任务栏图标启用
                    if (Setting2.IsTaskBarIconEnabled && (App.Current.NotifyIcon is not null))
                    {
                        // 自动启动 且设置了隐式初始化
                        if ((!App.IsLaunchedByUser) && Setting2.CloseMainWindowAfterInitializaion)
                        {
                            MainWindow.HasEverOpen = true;
                            mainWindow.Close();

                            // release PostInitializationTaskPreventer before return and eventually close window
                            // fix CloseMainWindowAfterInitializaion break initialization issue
                            PostInitializationTaskPreventer.Release();

                            // prevent later messenger call
                            return;
                        }
                    }
                }

                // 设置已经打开过 状态
                MainWindow.HasEverOpen = true;
                App.Messenger.Send(new PostInitializationCompletedMessage(mainWindow));

                await Task.Delay(TimeSpan.FromMilliseconds(800));
                if (!navigationService.HasEverNavigated)
                {
                    navigationService.Navigate<HomePage>(isSyncTabRequested: true);
                }

                PostInitializationTaskPreventer.Release();
            }
        }

        private void AddAdditionalNavigationViewItems()
        {
            // webview entries must add first
            navigationService.AddWebViewEntries(App.AutoWired<WebViewLobbyViewModel>().Entries);

            // then we add pilugin pages
            App.Current.PluginService.Plugins.ForEach(plugin =>
            plugin.ForEachAttribute<ImportPageAttribute>(importPage =>
            navigationService.AddToNavigation(importPage)));
        }

        private async Task CheckUpdateForNotificationAsync()
        {
            IUpdateService updateService = App.AutoWired<IUpdateService>();
            switch (await updateService.CheckUpdateStateAsync())
            {
                case UpdateState.NeedUpdate:
                    {
                        new ToastContentBuilder()
                            .AddText("有新的更新可用")
                            .AddText(updateService.NewVersion?.ToString())
                            .AddButton(new ToastButton()
                                .SetContent("更新")
                                .AddArgument("action", "update")
                                .SetBackgroundActivation())
                            .AddButton(new ToastButtonDismiss("忽略"))
                            .SafeShow();
                        break;
                    }

                case UpdateState.NotAvailable:
                    {
                        new ToastContentBuilder()
                            .AddText("检查更新失败")
                            .AddText("无法连接到 Github")
                            .SafeShow();
                        break;
                    }

                case UpdateState.IsNewestRelease:
                case UpdateState.IsInsiderVersion:
                default:
                    break;
            }
        }

        private async Task TrySignInOnStartUpAsync()
        {
            if (Setting2.AutoDailySignInOnLaunch)
            {
                if ((DateTime.UtcNow.AddHours(8)).Date > Setting2.LastAutoSignInTime)
                {
                    await App.AutoWired<ISignInService>().TrySignAllAccountsRolesInAsync();
                }
            }
        }

        private void TryInitializeTaskbarIcon()
        {
            if (Setting2.IsTaskBarIconEnabled.Get() && App.Current.NotifyIcon is null)
            {
                App.Current.NotifyIcon = App.Current.FindResource("TaskbarIcon") as TaskbarIcon;
                App.Current.NotifyIcon!.DataContext = App.AutoWired<TaskbarIconViewModel>();
            }
        }
    }
}