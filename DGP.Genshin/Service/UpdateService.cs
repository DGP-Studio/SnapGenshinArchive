using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.DataModel.Updating;
using DGP.Genshin.Helper.Extension;
using DGP.Genshin.Message;
using DGP.Genshin.Service.Abstraction.Setting;
using DGP.Genshin.Service.Abstraction.Updating;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.Threading;
using Octokit;
using Snap.Core.DependencyInjection;
using Snap.Data.Json;
using Snap.Data.Utility;
using Snap.Net.Download;
using Snap.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace DGP.Genshin.Service
{
    /// <inheritdoc cref="IUpdateService"/>
    [Service(typeof(IUpdateService), InjectAs.Singleton)]
    internal class UpdateService : IUpdateService
    {
        private const string UpdateNotificationTag = "snap_genshin_update";
        private const string UpdaterExecutable = "DGP.Genshin.Updater.exe";
        private const string UpdaterFolder = "Updater";

        private readonly IMessenger messenger;
        private readonly TaskPreventer updateTaskPreventer = new();
        private NotificationUpdater? notificationUpdater;

        /// <summary>
        /// 构造一个新的更新服务
        /// </summary>
        /// <param name="messenger">消息器</param>
        public UpdateService(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <inheritdoc/>
        public Uri? PackageUri { get; private set; }

        /// <inheritdoc/>
        public Version? NewVersion { get; private set; }

        /// <inheritdoc/>
        public string? ReleaseNote { get; private set; }

        /// <inheritdoc/>
        public Version CurrentVersion
        {
            get => App.Current.Version;
        }

        private Downloader? InnerDownloader { get; set; }

        /// <inheritdoc/>
        public async Task<UpdateState> CheckUpdateStateAsync()
        {
            try
            {
                IUpdateChecker updateChecker = Setting2.UpdateAPI.Get() switch
                {
                    UpdateAPI.PatchAPI => new PatchUpdateChecker(),
                    UpdateAPI.GithubAPI => new GithubUpdateChecker(),
                    _ => throw Must.NeverHappen(),
                };

                UpdateInfomation? info = await updateChecker.GetUpdateInfomationAsync();
                if (info != null)
                {
                    ReleaseNote = info.ReleaseNote;
                    PackageUri = info.PackageUrl;
                    NewVersion = new Version(info.Version);
                }
                else
                {
                    return UpdateState.NotAvailable;
                }

                if (Debugger.IsAttached)
                {
                    return UpdateState.NeedUpdate;
                }
                else
                {
                    if (NewVersion > CurrentVersion)
                    {
                        return UpdateState.NeedUpdate;
                    }
                    else
                    {
                        if (NewVersion == CurrentVersion)
                        {
                            return UpdateState.IsNewestRelease;
                        }
                        else
                        {
                            return UpdateState.IsInsiderVersion;
                        }
                    }
                }
            }
            catch
            {
                return UpdateState.NotAvailable;
            }
        }

        /// <inheritdoc/>
        public async Task DownloadAndInstallPackageAsync()
        {
            if (updateTaskPreventer.ShouldExecute)
            {
                string destinationPath = PathContext.Locate("Package.zip");

                Must.NotNull(PackageUri!);
                Must.NotNull(NewVersion!);

                InnerDownloader = new(PackageUri, destinationPath);
                notificationUpdater = new(NewVersion.ToString(), messenger);
                IProgress<DownloadInfomation> progress = new Progress<DownloadInfomation>(notificationUpdater.OnProgressChanged);

                // toast can only be shown & updated by main thread
                notificationUpdater.ShowDownloadToastNotification();

                bool caught = false;
                try
                {
                    await InnerDownloader.DownloadAsync(progress);
                }
                catch
                {
                    caught = true;
                }
                finally
                {
                    messenger.Send(UpdateProgressedMessage.Default);
                }

                if (caught)
                {
                    new ToastContentBuilder()
                    .AddText("下载更新时遇到问题")
                    .AddText("点击检查更新再次尝试")
                    .SafeShow();
                }
                else
                {
                    StartInstallUpdate();
                }

                updateTaskPreventer.Release();
            }
        }

        /// <summary>
        /// 开始安装更新
        /// </summary>
        private void StartInstallUpdate()
        {
            Directory.CreateDirectory(PathContext.Locate(UpdaterFolder));
            PathContext.MoveToFolderOrIgnore(UpdaterExecutable, UpdaterFolder);
            string oldUpdaterPath = PathContext.Locate(UpdaterFolder, UpdaterExecutable);

            if (File.Exists(oldUpdaterPath))
            {
                try
                {
                    // Updater自带工作路径纠正
                    Process.Start(new ProcessStartInfo()
                    {
                        // fix auth exception
                        Verb = "runas",
                        UseShellExecute = true,
                        FileName = oldUpdaterPath,
                        Arguments = "UpdateInstall",
                    });
                    this.ExecuteOnUI(App.Current.Shutdown);
                }
                catch (Win32Exception)
                {
                    new ToastContentBuilder()
                    .AddText("已经取消更新")
                    .AddText("下次更新需要重新下载安装包")
                    .SafeShow();
                }
            }
            else
            {
                new ToastContentBuilder()
                .AddText("在默认路径上未找到更新器")
                .AddText("请尝试手动解压安装包更新")
                .SafeShow();
            }
        }

        /// <summary>
        /// Github API 更新检查器实现
        /// </summary>
        internal class GithubUpdateChecker : IUpdateChecker
        {
            /// <inheritdoc/>
            public async Task<UpdateInfomation?> GetUpdateInfomationAsync()
            {
                try
                {
                    GitHubClient client = new(new ProductHeaderValue("SnapGenshin"))
                    {
                        Credentials = new Credentials(GithubToken.GetToken()),
                    };
                    Release? release = await client.Repository.Release.GetLatest("DGP-Studio", "Snap.Genshin");
                    return new()
                    {
                        ReleaseNote = release.Body,
                        PackageUrl = new Uri(release.Assets[0].BrowserDownloadUrl),
                        Version = release.TagName,
                    };
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// because repo cant cantain original token string
            /// so we store base64 encoded value here
            /// https://github.com/settings/tokens
            /// </summary>
            internal class GithubToken : Base64Converter
            {
                private GithubToken()
                {
                }

                /// <summary>
                /// 获取Github令牌
                /// </summary>
                /// <returns>Github令牌</returns>
                public static string GetToken()
                {
                    return Base64Decode(Encoding.UTF8, "Z2hwX3lDRWdVTVNaNnRRV2JpNjZMUWYyTUprbWFQVFI3bTEwYkVnTw==");
                }
            }
        }

        /// <summary>
        /// Patch API 更新检查器实现
        /// </summary>
        internal class PatchUpdateChecker : IUpdateChecker
        {
            /// <inheritdoc/>
            public async Task<UpdateInfomation?> GetUpdateInfomationAsync()
            {
                return await Json.FromWebsiteAsync<UpdateInfomation>("https://patch.snapgenshin.com/getPatch");
            }
        }

        /// <summary>
        /// 通知更新器
        /// </summary>
        internal class NotificationUpdater
        {
            private readonly string progressTitle;
            private readonly IMessenger messenger;

            private NotificationUpdateResult lastNotificationUpdateResult = NotificationUpdateResult.Succeeded;

            /// <summary>
            /// 构造一个新的通知更新器
            /// </summary>
            /// <param name="progressTitle">进度标题</param>
            /// <param name="messenger">消息器</param>
            public NotificationUpdater(string progressTitle, IMessenger messenger)
            {
                this.progressTitle = progressTitle;
                this.messenger = messenger;
            }

            /// <summary>
            /// 显示下载进度通知
            /// </summary>
            internal void ShowDownloadToastNotification()
            {
                lastNotificationUpdateResult = NotificationUpdateResult.Succeeded;
                new ToastContentBuilder()
                    .AddText("下载更新中...")
                    .AddVisualChild(new AdaptiveProgressBar()
                    {
                        Title = progressTitle,
                        Value = new BindableProgressBarValue("progressValue"),
                        ValueStringOverride = new BindableString("progressValueString"),
                        Status = new BindableString("progressStatus"),
                    })
                    .SafeShow(toast =>
                    {
                        toast.Tag = UpdateNotificationTag;
                        toast.Data = new(
                            new Dictionary<string, string>()
                            {
                                { "progressValue", "0" },
                                { "progressValueString", "0% - 0MB / 0MB" },
                                { "progressStatus", "下载中..." },
                            },
                            0);
                    });
            }

            /// <summary>
            /// 进度更新
            /// </summary>
            /// <param name="downloadInfomation">下载信息</param>
            internal void OnProgressChanged(DownloadInfomation downloadInfomation)
            {
                // message will be sent anyway.
                string valueString = downloadInfomation.ToString();
                messenger.Send(new UpdateProgressedMessage(downloadInfomation));

                // if user has dismissed the notification, we don't update it anymore
                if (lastNotificationUpdateResult is NotificationUpdateResult.Succeeded)
                {
                    // notification could only be updated by main thread.
                    this.ExecuteOnUI(() => UpdateNotificationValue(downloadInfomation));
                }
            }

            /// <summary>
            /// 更新下载进度通知上的进度条
            /// </summary>
            /// <param name="downloadInfomation">下载信息</param>
            private void UpdateNotificationValue(DownloadInfomation downloadInfomation)
            {
                NotificationData data = new() { SequenceNumber = 0 };

                data.Values["progressValue"] = $"{downloadInfomation.Percent}";
                data.Values["progressValueString"] = downloadInfomation.ToString();
                if (!downloadInfomation.IsDownloading)
                {
                    data.Values["progressStatus"] = "下载完成";
                }

                // Update the existing notification's data
                lastNotificationUpdateResult = ToastNotificationManagerCompat
                    .CreateToastNotifier()
                    .Update(data, UpdateNotificationTag);
            }
        }
    }
}