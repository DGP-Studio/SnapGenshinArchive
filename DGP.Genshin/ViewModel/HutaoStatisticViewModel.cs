using CommunityToolkit.Mvvm.ComponentModel;
using DGP.Genshin.Control.GenshinElement.HutaoStatistic;
using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.DataModel.HutaoAPI;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.Helper.Extension;
using DGP.Genshin.HutaoAPI.GetModel;
using DGP.Genshin.HutaoAPI.PostModel;
using DGP.Genshin.MiHoYoAPI.GameRole;
using DGP.Genshin.MiHoYoAPI.Response;
using DGP.Genshin.Service.Abstraction;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Data.Primitive;
using Snap.Extenion.Enumerable;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 胡桃数据库视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class HutaoStatisticViewModel : ObservableObject, ISupportCancellation
    {
        private readonly ICookieService cookieService;
        private readonly IHutaoStatisticService hutaoStatisticService;

        private bool shouldUIPresent;
        private Overview? overview;
        private IList<Indexed<int, Item<double>>>? avatarParticipation2s;
        private IList<Item<IList<NamedValue<Rate<IList<Item<int>>>>>>>? avatarReliquaryUsages;
        private IList<Item<IList<Item<double>>>>? teamCollocations;
        private IList<Item<IList<Item<double>>>>? weaponUsages;
        private IList<Rate<Item<IList<NamedValue<double>>>>>? avatarConstellations;
        private IList<Indexed<int, Rate<Two<IList<HutaoItem>>>>>? teamCombinations;
        private bool periodUploaded;
        private Two<Item<Rank>>? rank;

        /// <summary>
        /// 构造一个新的胡桃数据库视图模型
        /// </summary>
        /// <param name="cookieService">cookie服务</param>
        /// <param name="hutaoStatisticService">胡桃数据库服务</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        public HutaoStatisticViewModel(ICookieService cookieService, IHutaoStatisticService hutaoStatisticService, IAsyncRelayCommandFactory asyncRelayCommandFactory)
        {
            this.cookieService = cookieService;
            this.hutaoStatisticService = hutaoStatisticService;

            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
            UploadCommand = asyncRelayCommandFactory.Create(UploadRecordsAsync);
        }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 指示数据界面是否可见
        /// </summary>
        public bool ShouldUIPresent
        {
            get => shouldUIPresent;

            set => SetProperty(ref shouldUIPresent, value);
        }

        /// <summary>
        /// 玩家计数总览
        /// </summary>
        public Overview? Overview
        {
            get => overview;

            set => SetProperty(ref overview, value);
        }

        /// <summary>
        /// 当前是否已经上传
        /// </summary>
        public bool PeriodUploaded
        {
            get => periodUploaded;
            set => SetProperty(ref periodUploaded, value);
        }

        /// <summary>
        /// 角色出场率
        /// </summary>
        public IList<Indexed<int, Item<double>>>? AvatarParticipation2s
        {
            get => avatarParticipation2s;

            set => SetProperty(ref avatarParticipation2s, value);
        }

        /// <summary>
        /// 角色圣遗物搭配
        /// </summary>
        public IList<Item<IList<NamedValue<Rate<IList<Item<int>>>>>>>? AvatarReliquaryUsages
        {
            get => avatarReliquaryUsages;

            set => SetProperty(ref avatarReliquaryUsages, value);
        }

        /// <summary>
        /// 角色搭配
        /// </summary>
        public IList<Item<IList<Item<double>>>>? TeamCollocations
        {
            get => teamCollocations;

            set => SetProperty(ref teamCollocations, value);
        }

        /// <summary>
        /// 武器使用
        /// </summary>
        public IList<Item<IList<Item<double>>>>? WeaponUsages
        {
            get => weaponUsages;

            set => SetProperty(ref weaponUsages, value);
        }

        /// <summary>
        /// 角色命座
        /// </summary>
        public IList<Rate<Item<IList<NamedValue<double>>>>>? AvatarConstellations
        {
            get => avatarConstellations;

            set => SetProperty(ref avatarConstellations, value);
        }

        /// <summary>
        /// 队伍出场
        /// </summary>
        public IList<Indexed<int, Rate<Two<IList<HutaoItem>>>>>? TeamCombinations
        {
            get => teamCombinations;

            set => SetProperty(ref teamCombinations, value);
        }

        /// <summary>
        /// 排行
        /// </summary>
        public Two<Item<Rank>>? Rank
        {
            get => rank;
            set => SetProperty(ref rank, value);
        }

        /// <summary>
        /// 打开界面触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        /// <summary>
        /// 上传数据命令
        /// </summary>
        public ICommand UploadCommand { get; }

        private async Task OpenUIAsync()
        {
            try
            {
                await hutaoStatisticService.InitializeAsync(CancellationToken);

                Overview = await hutaoStatisticService.GetOverviewAsync(CancellationToken);

                // PeriodUploaded
                await UpdatePreiodUploadedAsync();

                // V1
                AvatarParticipation2s = hutaoStatisticService.GetAvatarParticipation2s();
                TeamCollocations = hutaoStatisticService.GetTeamCollocations();
                WeaponUsages = hutaoStatisticService.GetWeaponUsages();

                // V2
                AvatarReliquaryUsages = hutaoStatisticService.GetReliquaryUsages();
                AvatarConstellations = hutaoStatisticService.GetAvatarConstellations();
                TeamCombinations = hutaoStatisticService.GetTeamCombinations();
            }
            catch (TaskCanceledException)
            {
                this.Log("Open UI cancelled");
            }
            catch (Exception e)
            {
                this.Log(e);
                await new ContentDialog()
                {
                    Title = "加载失败",
                    Content = e.Message,
                    PrimaryButtonText = "确定",
                    DefaultButton = ContentDialogButton.Primary,
                }.ShowAsync().ConfigureAwait(false);
                return;
            }

            ShouldUIPresent = true;
        }

        private async Task UpdatePreiodUploadedAsync()
        {
            List<UserGameRole> gameRoles = await new UserGameRoleProvider(cookieService.CurrentCookie)
                .GetUserGameRolesAsync(CancellationToken);
            UserGameRole? role = gameRoles.MatchedOrFirst(role => role.IsChosen);
            PeriodUploaded = await hutaoStatisticService.GetPeriodUploadedAsync(Must.NotNull(role!.GameUid!), CancellationToken);
            Rank = await hutaoStatisticService.GetRankAsync(Must.NotNull(role!.GameUid!), CancellationToken);
        }

        private async Task UploadRecordsAsync()
        {
            ShouldUIPresent = false;
            try
            {
                await hutaoStatisticService.GetAllRecordsAndUploadAsync(cookieService.CurrentCookie, ConfirmAsync, HandleResponseAsync, CancellationToken);
            }
            catch (TaskCanceledException)
            {
                this.Log("upload data cancelled by user switch page");
            }
            catch (Exception e)
            {
                this.Log(e);
                await new ContentDialog()
                {
                    Title = "提交记录失败",
                    Content = "发生了致命错误",
                    PrimaryButtonText = "确定",
                    DefaultButton = ContentDialogButton.Primary,
                }.ShowAsync();
            }
            finally
            {
                ShouldUIPresent = true;
            }
        }

        private async Task<bool> ConfirmAsync(PlayerRecord record)
        {
            return await this.ExecuteOnUIAsync(new UploadDialog(record).ShowAsync) == ContentDialogResult.Primary;
        }

        private async Task HandleResponseAsync(Response resonse)
        {
            await new ContentDialog()
            {
                Title = "提交记录",
                Content = resonse.Message,
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary,
            }.ShowAsync();

            await UpdatePreiodUploadedAsync();
        }
    }
}