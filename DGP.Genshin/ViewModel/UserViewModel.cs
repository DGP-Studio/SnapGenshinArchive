using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.DataModel.Cookie;
using DGP.Genshin.DataModel.DailyNote;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.Helper.Extension;
using DGP.Genshin.Message;
using DGP.Genshin.MiHoYoAPI.GameRole;
using DGP.Genshin.MiHoYoAPI.Journey;
using DGP.Genshin.MiHoYoAPI.Record.DailyNote;
using DGP.Genshin.MiHoYoAPI.UserInfo;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.Setting;
using Microsoft.AppCenter.Crashes;
using Microsoft.VisualStudio.Threading;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Core.Mvvm;
using Snap.Data.Primitive;
using Snap.Extenion.Enumerable;
using Snap.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 账户管理视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class UserViewModel : ObservableRecipient2, ISupportCancellation,
        IRecipient<CookieAddedMessage>,
        IRecipient<CookieRemovedMessage>,
        IRecipient<DailyNotesRefreshedMessage>
    {
        private readonly ICookieService cookieService;
        private readonly IDailyNoteService dailynoteService;
        private readonly IMessenger messenger;

        private ObservableCollection<CookieUserInfo> cookieUserInfos = new();
        private CookieUserInfo? selectedCookieUserInfo;
        private List<CookieUserGameRole>? cookieUserGameRoles;
        private CookieUserGameRole? selectedCookieUserGameRole;
        private DailyNote? dailyNote;
        private DailyNoteNotifyConfiguration dailyNoteNotifyConfiguration;
        private JourneyInfo? journeyInfo;
        private bool autoDailySignInOnLaunch;
        private bool signInSilently;
        private NamedValue<TimeSpan> selectedResinAutoRefreshTime;

        /// <summary>
        /// 构造一个新的用户视图模型
        /// </summary>
        /// <param name="cookieService">cookie服务</param>
        /// <param name="dailyNoteService">实时便笺服务</param>
        /// <param name="signInService">签到服务</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        /// <param name="messenger">消息器</param>
        public UserViewModel(
            ICookieService cookieService,
            IDailyNoteService dailyNoteService,
            ISignInService signInService,
            IAsyncRelayCommandFactory asyncRelayCommandFactory,
            IMessenger messenger)
            : base(messenger)
        {
            this.cookieService = cookieService;
            dailynoteService = dailyNoteService;
            this.messenger = messenger;

            // 实时树脂
            DailyNoteNotifyConfiguration = Setting2.DailyNoteNotifyConfiguration.GetNonValueType(() => new());
            selectedResinAutoRefreshTime = ResinAutoRefreshTimes
                .First(s => s.Value.TotalMinutes == Setting2.ResinRefreshMinutes);

            // 签到选项
            AutoDailySignInOnLaunch = Setting2.AutoDailySignInOnLaunch;
            SignInSilently = Setting2.SignInSilently;
            SignInImmediatelyCommand = asyncRelayCommandFactory.Create(signInService.TrySignAllAccountsRolesInAsync);

            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
            RemoveUserCommand = asyncRelayCommandFactory.Create(RemoveUserAsync);
            AddUserCommand = asyncRelayCommandFactory.Create(AddUserAsync);
            RefreshCommand = new RelayCommand(RefreshUI);
            CopyCookieCommand = new RelayCommand(CopyCookie);
        }

        /// <summary>
        /// 是否自动签到
        /// </summary>
        public bool AutoDailySignInOnLaunch
        {
            get => autoDailySignInOnLaunch;

            set
            {
                Setting2.AutoDailySignInOnLaunch.Set(value, false);
                SetProperty(ref autoDailySignInOnLaunch, value);
            }
        }

        /// <summary>
        /// 静默签到
        /// </summary>
        public bool SignInSilently
        {
            get => signInSilently;

            set => SetPropertyAndCallbackOnCompletion(ref signInSilently, value, Setting2.SignInSilently.Set);
        }

        /// <summary>
        /// 立即签到命令
        /// </summary>
        public ICommand SignInImmediatelyCommand { get; }

        /// <summary>
        /// 树脂自动刷新时间选项
        /// </summary>
        public List<NamedValue<TimeSpan>> ResinAutoRefreshTimes { get; } = new()
        {
            new("4 分钟 | 0.5 树脂", TimeSpan.FromMinutes(4)),
            new("8 分钟 | 1 树脂", TimeSpan.FromMinutes(8)),
            new("30 分钟 | 3.75 树脂", TimeSpan.FromMinutes(30)),
            new("40 分钟 | 5 树脂", TimeSpan.FromMinutes(40)),
            new("1 小时 | 7.5 树脂", TimeSpan.FromMinutes(60)),
        };

        /// <summary>
        /// 当前选中的树脂刷新时间
        /// </summary>
        public NamedValue<TimeSpan> SelectedResinAutoRefreshTime
        {
            get => selectedResinAutoRefreshTime;

            set => SetPropertyAndCallbackOnCompletion(ref selectedResinAutoRefreshTime, value, v => Setting2.ResinRefreshMinutes.Set(v.Value.TotalMinutes));
        }

        /// <summary>
        /// 用户信息列表
        /// </summary>
        public ObservableCollection<CookieUserInfo> CookieUserInfos
        {
            get => cookieUserInfos;

            set => SetProperty(ref cookieUserInfos, value);
        }

        /// <summary>
        /// 选中的用户
        /// </summary>
        public CookieUserInfo? SelectedCookieUserInfo
        {
            get => selectedCookieUserInfo;

            set => SetPropertyAndCallbackOnCompletion(ref selectedCookieUserInfo, value, OnSelectedCookieUserInfoChangedAsync);
        }

        /// <summary>
        /// 用户角色信息列表
        /// </summary>
        public List<CookieUserGameRole>? CookieUserGameRoles
        {
            get => cookieUserGameRoles;

            set => SetProperty(ref cookieUserGameRoles, value);
        }

        /// <summary>
        /// 选中的用户角色信息
        /// </summary>
        public CookieUserGameRole? SelectedCookieUserGameRole
        {
            get => selectedCookieUserGameRole;

            set => SetPropertyAndCallbackOnCompletion(ref selectedCookieUserGameRole, value, OnSelectedCookieUserGameRoleChangedAsync);
        }

        /// <summary>
        /// 当前用户角色的实时便笺
        /// </summary>
        public DailyNote? DailyNote
        {
            get => dailyNote;

            set => SetProperty(ref dailyNote, value);
        }

        /// <summary>
        /// 实时便笺通知开关
        /// </summary>
        public DailyNoteNotifyConfiguration DailyNoteNotifyConfiguration
        {
            get => dailyNoteNotifyConfiguration;

            [MemberNotNull(nameof(dailyNoteNotifyConfiguration))]
            set => SetProperty(ref dailyNoteNotifyConfiguration, value);
        }

        /// <summary>
        /// 旅行札记信息
        /// </summary>
        public JourneyInfo? JourneyInfo
        {
            get => journeyInfo;

            set => SetProperty(ref journeyInfo, value);
        }

        /// <summary>
        /// 打开界面时触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        /// <summary>
        /// 移除用户命令
        /// </summary>
        public ICommand RemoveUserCommand { get; }

        /// <summary>
        /// 添加用户命令
        /// </summary>
        public ICommand AddUserCommand { get; }

        /// <summary>
        /// 刷新实施便笺命令
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// 复制Cookie命令
        /// </summary>
        public ICommand CopyCookieCommand { get; }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; }

        /// <inheritdoc/>
        public void Receive(CookieAddedMessage message)
        {
            string newCookie = message.Value;
            AddCookieUserInfoAsync(newCookie).Forget();
        }

        /// <inheritdoc/>
        public void Receive(CookieRemovedMessage message)
        {
            RemoveCookieUserInfo(message);
        }

        /// <inheritdoc/>
        public void Receive(DailyNotesRefreshedMessage message)
        {
            this.Log("daily note updated");
            UpdateDailyNote(SelectedCookieUserGameRole);
        }

        private async Task OpenUIAsync()
        {
            try
            {
                foreach (string cookie in cookieService.Cookies)
                {
                    if (await new UserInfoProvider(cookie).GetUserInfoAsync(CancellationToken) is UserInfo info)
                    {
                        CookieUserInfos.Add(new CookieUserInfo(cookie, info));
                    }
                }

                SelectedCookieUserInfo = CookieUserInfos.FirstOrDefault(c => c.Cookie == cookieService.CurrentCookie);
            }
            catch (TaskCanceledException)
            {
                this.Log("Open UI canceled");
            }
        }

        private async Task AddUserAsync()
        {
            await cookieService.AddCookieToPoolOrIgnoreAsync();
            RefreshUI();
        }

        private async Task RemoveUserAsync()
        {
            if (cookieService.Cookies.Count <= 1)
            {
                await new ContentDialog()
                {
                    Title = "删除用户失败",
                    Content = "我们需要至少一个用户的信息才能使程序正常运转。",
                    PrimaryButtonText = "确定",
                    DefaultButton = ContentDialogButton.Primary,
                }.ShowAsync();

                // fix remove cookie crash.
                return;
            }

            if (SelectedCookieUserInfo is not null)
            {
                ContentDialogResult result = await new ContentDialog()
                {
                    Title = "确定要删除该用户吗?",
                    Content = "删除用户的操作不可撤销。",
                    PrimaryButtonText = "确定",
                    SecondaryButtonText = "取消",
                    DefaultButton = ContentDialogButton.Primary,
                }.ShowAsync();

                if (result is ContentDialogResult.Primary)
                {
                    cookieService.Cookies.Remove(SelectedCookieUserInfo.Cookie);
                }
            }
        }

        private void RefreshUI()
        {
            messenger.Send(new UserRequestRefreshMessage());
        }

        private async Task AddCookieUserInfoAsync(string newCookie)
        {
            try
            {
                this.Log("new Cookie added");
                if (await new UserInfoProvider(newCookie).GetUserInfoAsync() is UserInfo newInfo)
                {
                    this.ExecuteOnUI(() => CookieUserInfos.Add(new CookieUserInfo(newCookie, newInfo)));
                }

                this.Log(cookieUserInfos.Count);
            }
            catch (Exception ex)
            {
                this.Log(ex);
                Crashes.TrackError(ex);
            }
        }

        private void RemoveCookieUserInfo(CookieRemovedMessage message)
        {
            this.Log("Cookie removed");
            CookieUserInfo? prevSelected = SelectedCookieUserInfo;
            CookieUserInfo? currentRemoved = CookieUserInfos.First(u => u.Cookie == message.Value);

            CookieUserInfos.Remove(currentRemoved);

            if (prevSelected == currentRemoved)
            {
                SelectedCookieUserInfo = CookieUserInfos.First();
            }

            this.Log(cookieUserInfos.Count);
        }

        [PropertyChangedCallback]
        private async Task OnSelectedCookieUserInfoChangedAsync(CookieUserInfo? cookieUserInfo)
        {
            try
            {
                if (cookieUserInfo != null)
                {
                    cookieService.ChangeOrIgnoreCurrentCookie(cookieUserInfo.Cookie);

                    // update user game roles
                    List<UserGameRole> userGameRoles = await new UserGameRoleProvider(cookieService.CurrentCookie)
                        .GetUserGameRolesAsync(CancellationToken);
                    CookieUserGameRoles = userGameRoles
                        .Select(role => new CookieUserGameRole(cookieUserInfo.Cookie, role))
                        .ToList();
                    SelectedCookieUserGameRole = CookieUserGameRoles.MatchedOrFirst(i => i.UserGameRole.IsChosen);
                }
            }
            catch (TaskCanceledException)
            {
                this.Log("OnSelectedCookieUserInfoChangedAsync canceled by user switch page");
            }
        }

        [PropertyChangedCallback]
        private async Task OnSelectedCookieUserGameRoleChangedAsync(CookieUserGameRole? cookieUserGameRole)
        {
            UpdateDailyNote(cookieUserGameRole);
            if (cookieUserGameRole is not null)
            {
                UserGameRole role = cookieUserGameRole.UserGameRole;
                JourneyInfo = await new JourneyProvider(cookieUserGameRole.Cookie).GetMonthInfoAsync(role);
            }
        }

        private void UpdateDailyNote(CookieUserGameRole? cookieUserGameRole)
        {
            if (cookieUserGameRole != null)
            {
                DailyNote = dailynoteService.GetDailyNote(cookieUserGameRole);
            }
        }

        public void CopyCookie()
        {
            if (SelectedCookieUserInfo is not null)
            {
                Clipboard.Clear();
                try
                {
                    Clipboard.SetText(SelectedCookieUserInfo.Cookie);
                }
                catch
                {
                    try
                    {
                        Clipboard2.SetText(SelectedCookieUserInfo.Cookie);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}