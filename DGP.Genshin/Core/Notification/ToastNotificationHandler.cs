using DGP.Genshin.Service.Abstraction.Launching;
using DGP.Genshin.Service.Abstraction.Setting;
using DGP.Genshin.Service.Abstraction.Updating;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.Threading;
using Snap.Core.Logging;
using Snap.Data.Json;
using System.Threading.Tasks;

namespace DGP.Genshin.Core.Notification
{
    /// <summary>
    /// 通知事件处理器
    /// </summary>
    internal sealed class ToastNotificationHandler
    {
        /// <summary>
        /// 在后台处理并响应通知
        /// </summary>
        /// <param name="toastArgs">通知事件参数</param>
        internal void OnActivatedByNotification(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            this.Log(Json.Stringify(toastArgs));
            ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
            foreach ((string key, string value) in args)
            {
                HandleActionUpdateAsync(key, value).Forget();
                HandleTaskbarHintHide(key, value);
                HandleLaunchValueAsync(key, value).Forget();
            }
        }

        private async Task HandleActionUpdateAsync(string key, string value)
        {
            if (key is "action" && value == "update")
            {
                IUpdateService updateService = App.AutoWired<IUpdateService>();
                if (updateService.PackageUri is not null)
                {
                    await updateService.DownloadAndInstallPackageAsync();
                }
                else
                {
                    new ToastContentBuilder()
                        .AddText("当前无法获取更新信息")
                        .AddText("请重启 Snap Genshin")
                        .SafeShow();
                }
            }
        }

        private void HandleTaskbarHintHide(string key, string value)
        {
            if (key is "taskbarhint" && value == "hide")
            {
                Setting2.IsTaskBarIconHintDisplay.Set(false);
            }
        }

        private async Task HandleLaunchValueAsync(string key, string value)
        {
            if (key is "launch")
            {
                ILaunchService launchService = App.Current.SwitchableImplementationManager.CurrentLaunchService!.Factory.Value;
                switch (value)
                {
                    case "game":
                        {
                            await launchService.LaunchAsync(LaunchOption.FromCurrentSettings(), ex =>
                            {
                                new ToastContentBuilder()
                                    .AddText("启动游戏失败")
                                    .AddText(ex.Message)
                                    .SafeShow();
                            });
                            break;
                        }

                    case "launcher":
                        {
                            launchService.OpenOfficialLauncher(ex =>
                            {
                                new ToastContentBuilder()
                                    .AddText("打开启动器失败")
                                    .AddText(ex.Message)
                                    .SafeShow();
                            });
                            break;
                        }

                    default:
                        break;
                }
            }
        }
    }
}