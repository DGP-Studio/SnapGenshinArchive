using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.DataModel.Achievement;
using DGP.Genshin.Message;
using DGP.Genshin.Message.Internal;
using DGP.Genshin.Page;
using DGP.Genshin.Service.Abstraction.Achievement;
using DGP.Genshin.Service.Abstraction.Launching;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.Threading;
using Snap.Core.Logging;
using Snap.Core.Mvvm.Messaging;
using Snap.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DGP.Genshin.Core.Activation
{
    /// <summary>
    /// 启动参数处理器
    /// </summary>
    internal sealed class LaunchHandler
    {
        private readonly TaskPreventer taskPreventer = new();

        /// <summary>
        /// 处理启动参数
        /// </summary>
        /// <param name="argument">参数</param>
        public void Handle(string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                this.Log($"无启动参数");

                // 用户正常启动，呼出主窗体
                App.BringWindowToFront<MainWindow>();
            }
            else
            {
                this.Log($"Activated with [{argument}]");

                if (Uri.TryCreate(argument, UriKind.Absolute, out Uri? argumentUri))
                {
                    UriBuilder uriBuilder = new(argumentUri);
                    if (uriBuilder.Scheme == "snapgenshin")
                    {
                        string category = uriBuilder.Host.ToLowerInvariant();
                        string action = uriBuilder.Path;

                        // switch navigation page target portion
                        switch (category)
                        {
                            case "achievement":
                                {
                                    HandleAchievementActionAsync(action, uriBuilder).Forget();
                                    break;
                                }

                            case "launch":
                                {
                                    HandleLaunchActionAsync(action).Forget();
                                    break;
                                }

                            default:
                                break;
                        }
                    }
                }
            }
        }

        private async Task HandleAchievementActionAsync(string action, UriBuilder uriBuilder)
        {
            if (taskPreventer.ShouldExecute)
            {
                this.Log("Request to open MainWindow.");
                Recipient<PostInitializationCompletedMessage>? recipient = new(App.Messenger);
                if (App.TryBringWindowToFront<MainWindow>())
                {
                    // 等待后初始化完成
                    if (!await recipient.WaitOneAsync())
                    {
                        new ToastContentBuilder()
                            .AddText("操作失败,在后台处理此操作时遇到问题。")
                            .Show();
                        taskPreventer.Release();
                        return;
                    }
                }
                else
                {
                    this.Log("MainWindow already opened.");
                }

                switch (action)
                {
                    case "/import/file":
                        {
                            // will be like: ?path="D://My folder/my file.json"
                            string path = Uri.UnescapeDataString(uriBuilder.Query);
                            this.Log($"Unescaped as : [{path}]");
                            Match match = new Regex("(?<=\\?path=)(.*)").Match(path);
                            path = match.Success ? match.Value : string.Empty;
                            path = path.Trim('"');
                            this.Log($"Matched as : [{path}]");

                            Func<IAchievementService, IEnumerable<IdTime>?> importer = (IAchievementService service) =>
                            {
                                string data = File.ReadAllText(path);
                                return service.TryGetImportData(data);
                            };

                            App.Messenger.Send(new NavigateRequestMessage(typeof(AchievementPage), true, importer));
                            break;
                        }

                    case "/import/clipboard":
                        {
                            Func<IAchievementService, IEnumerable<IdTime>?> importer = (IAchievementService service) =>
                            {
                                string data = Clipboard.GetText();
                                return service.TryGetImportData(data);
                            };

                            App.Messenger.Send(new NavigateRequestMessage(typeof(AchievementPage), true, importer));
                            break;
                        }

                    case "/import/uiaf":
                        {
                            Func<IAchievementService, IEnumerable<IdTime>?> importer = (IAchievementService service) =>
                            {
                                string data = Clipboard.GetText();
                                return service.TryGetImportData(ImportAchievementSource.UIAF, data);
                            };

                            App.Messenger.Send(new NavigateRequestMessage(typeof(AchievementPage), true, importer));
                            break;
                        }

                    default:
                        break;
                }

                taskPreventer.Release();
            }
        }

        private async Task HandleLaunchActionAsync(string action)
        {
            if (taskPreventer.ShouldExecute)
            {
                ILaunchService launchService = App.Current.SwitchableImplementationManager
                .CurrentLaunchService!.Factory.Value;
                switch (action)
                {
                    case "/game":
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

                    case "/launcher":
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

                taskPreventer.Release();
            }
        }
    }
}