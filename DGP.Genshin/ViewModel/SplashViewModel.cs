using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.Message.Internal;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.IntegrityCheck;
using Snap.Core.DependencyInjection;
using Snap.Core.Mvvm;
using Snap.Data.Primitive;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 启动界面视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class SplashViewModel : ObservableObject2
    {
        private static bool hasEverCheckMetadata = false;

        private readonly ICookieService cookieService;
        private readonly IIntegrityCheckService integrityCheckService;
        private readonly IMessenger messenger;

        private bool hasEverSend;

        private bool isCookieVisible = false;
        private bool isSplashNotVisible = false;
        private string currentStateDescription = "初始化...";
        private int currentCount;
        private string? currentInfo;
        private int? totalCount;
        private double percent;

        /// <summary>
        /// 构造一个新的启动界面视图模型
        /// </summary>
        /// <param name="cookieService">cookie服务</param>
        /// <param name="integrityCheckService">完整性检查服务</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        /// <param name="messenger">消息器</param>
        public SplashViewModel(
            ICookieService cookieService,
            IIntegrityCheckService integrityCheckService,
            IAsyncRelayCommandFactory asyncRelayCommandFactory,
            IMessenger messenger)
        {
            this.cookieService = cookieService;
            this.integrityCheckService = integrityCheckService;
            this.messenger = messenger;

            hasEverSend = false;

            SetCookieCommand = asyncRelayCommandFactory.Create(SetCookieAsync);
            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
        }

        /// <summary>
        /// 指示设置cookie功能是否可见
        /// </summary>
        public bool IsCookieVisible
        {
            get => isCookieVisible;

            set => SetPropertyAndCallbackOnCompletion(ref isCookieVisible, value, TrySendCompletedMessage);
        }

        /// <summary>
        /// 设置为 <see cref="true"/> 以触发淡入主界面动画
        /// </summary>
        public bool IsSplashNotVisible
        {
            get => isSplashNotVisible;

            set => SetProperty(ref isSplashNotVisible, value);
        }

        /// <summary>
        /// 当前状态提示文本
        /// </summary>
        public string CurrentStateDescription
        {
            get => currentStateDescription;

            set => SetProperty(ref currentStateDescription, value);
        }

        /// <summary>
        /// 当前检查的个数
        /// </summary>
        public int CurrentCount
        {
            get => currentCount;

            set => SetProperty(ref currentCount, value);
        }

        /// <summary>
        /// 当前检查的提示文本
        /// </summary>
        public string? CurrentInfo
        {
            get => currentInfo;

            set => SetProperty(ref currentInfo, value);
        }

        /// <summary>
        /// 检查的总个数
        /// </summary>
        public int? TotalCount
        {
            get => totalCount;

            set => SetProperty(ref totalCount, value);
        }

        /// <summary>
        /// 进度
        /// </summary>
        public double Percent
        {
            get => percent;

            set => SetProperty(ref percent, value);
        }

        /// <summary>
        /// 完整性检查监视器
        /// </summary>
        public WorkWatcher IntegrityChecking { get; set; } = new();

        /// <summary>
        /// 打开界面触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        /// <summary>
        /// 设置Cookie命令
        /// </summary>
        public ICommand SetCookieCommand { get; }

        /// <summary>
        /// 通知 <see cref="SplashViewModel"/> 结束初始化
        /// </summary>
        public void CompleteInitialization()
        {
            CurrentStateDescription = "完成";
            IsSplashNotVisible = true;
        }

        private async Task SetCookieAsync()
        {
            await cookieService.SetCookieAsync();
            IsCookieVisible = !cookieService.IsCookieAvailable;
        }

        private async Task OpenUIAsync()
        {
            CurrentStateDescription = "等待网络连接...";
            await Network.WaitConnectionAsync();
            CurrentStateDescription = "校验 Cookie 有效性...";
            await PerformCookieServiceCheckAsync();
            CurrentStateDescription = "校验 缓存资源 完整性...";
            await PerformIntegrityServiceCheckAsync();
            CurrentStateDescription = string.Empty;
            TrySendCompletedMessage();
        }

        private async Task PerformCookieServiceCheckAsync()
        {
            await cookieService.InitializeAsync();
            IsCookieVisible = !cookieService.IsCookieAvailable;
        }

        private async Task PerformIntegrityServiceCheckAsync()
        {
            Progress<IIntegrityCheckService.IIntegrityCheckState> progress = new();
            progress.ProgressChanged += (_, state) =>
            {
                CurrentCount = state.CurrentCount;
                Percent = (state.CurrentCount * 1D / TotalCount) ?? 0D;
                TotalCount = state.TotalCount;
                CurrentInfo = state.Info;
            };

            using (IntegrityChecking.Watch())
            {
                if (!hasEverSend && !hasEverCheckMetadata)
                {
                    await integrityCheckService.CheckMetadataIntegrityAsync(progress);
                    hasEverCheckMetadata = true;
                }
            }
        }

        [PropertyChangedCallback]
        private void TrySendCompletedMessage()
        {
            if (!hasEverSend)
            {
                if (IsCookieVisible == false && IntegrityChecking.IsCompleted)
                {
                    messenger.Send(new SplashInitializationCompletedMessage(this));
                    hasEverSend = true;
                }
            }
        }
    }
}