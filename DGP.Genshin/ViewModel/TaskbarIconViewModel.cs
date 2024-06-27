using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Control.Launching;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.Launching;
using Microsoft.Toolkit.Uwp.Notifications;
using Snap.Core.DependencyInjection;
using Snap.Core.Mvvm;
using System.Linq;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 任务栏图标视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class TaskbarIconViewModel : ObservableRecipient2
    {
        private readonly ILaunchService launchService;
        private readonly ISignInService signInService;

        /// <summary>
        /// 构造一个新的任务栏图标视图模型
        /// </summary>
        /// <param name="signInService">签到服务</param>
        /// <param name="asyncRelayCommandFactory">异步工厂命令</param>
        /// <param name="messenger">消息器</param>
        public TaskbarIconViewModel(ISignInService signInService, IAsyncRelayCommandFactory asyncRelayCommandFactory, IMessenger messenger)
            : base(messenger)
        {
            launchService = App.Current.SwitchableImplementationManager.CurrentLaunchService!.Factory.Value;
            this.signInService = signInService;

            ShowMainWindowCommand = new RelayCommand(OpenMainWindow);
            ExitCommand = new RelayCommand(ExitApp);
            RestartAsElevatedCommand = new RelayCommand(App.RestartAsElevated);
            LaunchGameCommand = asyncRelayCommandFactory.Create(LaunchGameAsync);
            OpenLauncherCommand = new RelayCommand(OpenLauncher);
            SignInCommand = asyncRelayCommandFactory.Create(SignInAsync);
        }

        /// <summary>
        /// 打开主窗体命令
        /// </summary>
        public ICommand ShowMainWindowCommand { get; }

        /// <summary>
        /// 退出命令
        /// </summary>
        public ICommand ExitCommand { get; }

        /// <summary>
        /// 以管理员模式重启命令
        /// </summary>
        public ICommand RestartAsElevatedCommand { get; }

        /// <summary>
        /// 启动游戏命令
        /// </summary>
        public ICommand LaunchGameCommand { get; }

        /// <summary>
        /// 打开启动器命令
        /// </summary>
        public ICommand OpenLauncherCommand { get; }

        /// <summary>
        /// 签到命令
        /// </summary>
        public ICommand SignInCommand { get; }

        private void OpenMainWindow()
        {
            App.BringWindowToFront<MainWindow>();
        }

        private void ExitApp()
        {
            App.Current.Shutdown();
        }

        private async Task LaunchGameAsync()
        {
            LaunchViewModel launchViewModel = App.AutoWired<LaunchViewModel>();

            bool shouldShowDialog = ShouldShowAccountDialog(launchViewModel);
            bool shouldContinueLaunch = ShouldContinueLaunch(launchViewModel, shouldShowDialog);

            if (shouldContinueLaunch)
            {
                await launchService.LaunchAsync(LaunchOption.FromCurrentSettings(), ex =>
                {
                    new ToastContentBuilder()
                        .AddText("启动游戏失败")
                        .AddText(ex.Message)
                        .SafeShow();
                });
            }
        }

        private bool ShouldShowAccountDialog(LaunchViewModel launchViewModel)
        {
            bool shouldShowDialog = true;

            // 主窗体存在
            if (App.Current.Windows.OfType<MainWindow>().Any())
            {
                shouldShowDialog = false;
            }
            else
            {
                // 账号数量小于等于1
                if (launchViewModel.Accounts.Count <= 1)
                {
                    shouldShowDialog = false;
                }
            }

            return shouldShowDialog;
        }

        private bool ShouldContinueLaunch(LaunchViewModel launchViewModel, bool shouldShowDialog)
        {
            bool shouldContinueLaunch = true;

            if (shouldShowDialog)
            {
                // 用户未正常关闭对话框
                if (new AccountWindow(launchViewModel).ShowDialog() != true)
                {
                    shouldContinueLaunch = false;
                }
            }

            return shouldContinueLaunch;
        }

        private void OpenLauncher()
        {
            launchService.OpenOfficialLauncher(ex =>
            {
                new ToastContentBuilder()
                    .AddText("打开启动器失败")
                    .AddText(ex.Message)
                    .Show();
            });
        }

        private async Task SignInAsync()
        {
            await signInService.TrySignAllAccountsRolesInAsync();
        }
    }
}