using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DGP.Genshin.Control;
using DGP.Genshin.Control.GenshinElement;
using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.MiHoYoAPI.Announcement;
using DGP.Genshin.Service.Abstraction;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Data.Primitive;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 主页视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class HomeViewModel : ObservableObject, ISupportCancellation
    {
        private readonly IHomeService homeService;

        private AnnouncementWrapper? announcement;
        private string? manifesto;

        /// <summary>
        /// 构造一个主页视图模型
        /// </summary>
        /// <param name="homeService">主页服务</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        public HomeViewModel(IHomeService homeService, IAsyncRelayCommandFactory asyncRelayCommandFactory)
        {
            this.homeService = homeService;

            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
            OpenAnnouncementUICommand = new RelayCommand<string>(OpenAnnouncementUI);
        }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 公告
        /// </summary>
        public AnnouncementWrapper? Announcement
        {
            get => announcement;
            set => SetProperty(ref announcement, value);
        }

        /// <summary>
        /// Snap Genshin 公告
        /// </summary>
        public string? Manifesto
        {
            get => manifesto;
            set => SetProperty(ref manifesto, value);
        }

        /// <summary>
        /// 打开界面监视器
        /// </summary>
        public WorkWatcher OpeningUI { get; } = new(false);

        /// <summary>
        /// 打开界面触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        /// <summary>
        /// 打开公告UI触发的命令
        /// </summary>
        public ICommand OpenAnnouncementUICommand { get; }

        private async Task OpenUIAsync()
        {
            using (OpeningUI.Watch())
            {
                try
                {
                    Manifesto = await homeService.GetManifestoAsync(CancellationToken);
                    Announcement = await homeService.GetAnnouncementsAsync(OpenAnnouncementUICommand, CancellationToken);
                }
                catch (TaskCanceledException)
                {
                    this.Log("Open UI cancelled");
                }
            }
        }

        private void OpenAnnouncementUI(string? content)
        {
            if (WebView2Helper.IsSupported)
            {
                using (AnnouncementWindow? window = new(content))
                {
                    window.ShowDialog();
                }
            }
            else
            {
                new WebView2RuntimeWindow().ShowDialog();
            }
        }
    }
}