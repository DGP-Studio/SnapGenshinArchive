using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DGP.Genshin.Control.GenshinElement.PromotionCalculate;
using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.DataModel.Promotion;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.MiHoYoAPI.Calculation;
using DGP.Genshin.MiHoYoAPI.GameRole;
using DGP.Genshin.Service.Abstraction;
using Microsoft.VisualStudio.Threading;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Core.Mvvm;
using Snap.Extenion.Enumerable;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 养成计算视图模型
    /// 按 [已有角色等级] 排序 按[获取角色时间] 排序
    /// 按【角色稀有度】筛选 按【武器类型】筛选 按【好感度等级】筛选 按（元素）筛选
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class PromotionCalculateViewModel : ObservableRecipient2, ISupportCancellation
    {
        private const string AvatarTag = "Avatar";
        private const string SkillTag = "Skill";

        private readonly IMaterialListService materialListService;

        private readonly Calculator calculator;
        private readonly UserGameRoleProvider userGameRoleProvider;

        private Avatar? selectedAvatar;
        private Avatar? selectedFullAvatar;
        private AvatarDetailData? avatarDetailData;
        private AvatarDetailData? fullAvatarDetailData;
        private AvatarDetailData? fullWeaponAvatarDetailData;
        private bool isListEmpty;
        private Consumption? consumption = new();
        private Consumption? fullAvatarConsumption;
        private Consumption? fullWeaponConsumption;
        private IEnumerable<Avatar>? avatars;
        private IEnumerable<UserGameRole>? userGameRoles;
        private List<Avatar>? fullAvatars;
        private List<Weapon>? fullWeapons;
        private MaterialList? materialList;
        private UserGameRole? selectedUserGameRole;
        private Weapon? selectedFullWeapon;

        /// <summary>
        /// 构造一个新的养成计算视图模型
        /// </summary>
        /// <param name="materialListService">材料清单服务</param>
        /// <param name="cookieService">cookie服务</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        /// <param name="messenger">消息器</param>
        public PromotionCalculateViewModel(
            IMaterialListService materialListService,
            ICookieService cookieService,
            IAsyncRelayCommandFactory asyncRelayCommandFactory,
            IMessenger messenger)
            : base(messenger)
        {
            this.materialListService = materialListService;

            calculator = new(cookieService.CurrentCookie);
            userGameRoleProvider = new(cookieService.CurrentCookie);

            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
            CloseUICommand = new RelayCommand(CloseUI);

            ComputeCommand = asyncRelayCommandFactory.Create(ComputeAsync);
            FullAvatarComputeCommand = asyncRelayCommandFactory.Create(ComputerFullAvatarAsync);
            FullWeaponComputeCommand = asyncRelayCommandFactory.Create(ComputerFullWeaponAsync);

            AddCharacterMaterialCommand = asyncRelayCommandFactory.Create<string>(AddCharacterMaterialToListAsync);
            AddWeaponMaterialCommand = asyncRelayCommandFactory.Create(AddWeaponMaterialToListAsync);

            AddFullCharacterMaterialCommand = asyncRelayCommandFactory.Create<string>(AddFullCharacterMaterialToListAsync);
            AddFullWeaponMaterialCommand = asyncRelayCommandFactory.Create(AddFullWeaponMaterialToListAsync);

            EditMaterialCommand = asyncRelayCommandFactory.Create<CalculableConsume>(EditMaterialAsync);
            RemoveMaterialCommand = asyncRelayCommandFactory.Create<CalculableConsume>(RemoveMaterialFromListAsync);
        }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 选中的角色
        /// </summary>
        public Avatar? SelectedAvatar
        {
            get => selectedAvatar;

            set => SetPropertyAndCallbackOnCompletion(ref selectedAvatar, value, UpdateAvatarDetailDataAsync);
        }

        /// <summary>
        /// 选中的全角色
        /// </summary>
        public Avatar? SelectedFullAvatar
        {
            get => selectedFullAvatar;
            set => SetPropertyAndCallbackOnCompletion(ref selectedFullAvatar, value, UpdateFullAvatarDetailData);
        }

        /// <summary>
        /// 角色详细数据
        /// </summary>
        public AvatarDetailData? AvatarDetailData
        {
            get => avatarDetailData;

            set => SetProperty(ref avatarDetailData, value);
        }

        /// <summary>
        /// 全角色详细数据
        /// </summary>
        public AvatarDetailData? FullAvatarDetailData
        {
            get => fullAvatarDetailData;
            set => SetProperty(ref fullAvatarDetailData, value);
        }

        /// <summary>
        /// 全武器详细数据
        /// </summary>
        public AvatarDetailData? FullWeaponAvatarDetailData
        {
            get => fullWeaponAvatarDetailData;
            set => SetProperty(ref fullWeaponAvatarDetailData, value);
        }

        /// <summary>
        /// 材料清单是否为空
        /// </summary>
        public bool IsListEmpty { get => isListEmpty; set => SetProperty(ref isListEmpty, value); }

        /// <summary>
        /// 材料消耗
        /// </summary>
        public Consumption? Consumption
        {
            get => consumption;

            set => SetProperty(ref consumption, value);
        }

        /// <summary>
        /// 全角色消耗
        /// </summary>
        public Consumption? FullAvatarConsumption
        {
            get => fullAvatarConsumption;
            set => SetProperty(ref fullAvatarConsumption, value);
        }

        /// <summary>
        /// 全武器消耗
        /// </summary>
        public Consumption? FullWeaponConsumption
        {
            get => fullWeaponConsumption;
            set => SetProperty(ref fullWeaponConsumption, value);
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        public IEnumerable<Avatar>? Avatars
        {
            get => avatars;

            set => SetProperty(ref avatars, value);
        }

        /// <summary>
        /// 玩家集合
        /// </summary>
        public IEnumerable<UserGameRole>? UserGameRoles
        {
            get => userGameRoles;

            set => SetProperty(ref userGameRoles, value);
        }

        /// <summary>
        /// 全角色列表
        /// </summary>
        public List<Avatar>? FullAvatars
        {
            get => fullAvatars;
            set => SetProperty(ref fullAvatars, value);
        }

        /// <summary>
        /// 全武器列表
        /// </summary>
        public List<Weapon>? FullWeapons
        {
            get => fullWeapons;
            set => SetProperty(ref fullWeapons, value);
        }

        /// <summary>
        /// 材料清单
        /// </summary>
        public MaterialList? MaterialList
        {
            get => materialList;

            set => SetProperty(ref materialList, value);
        }

        /// <summary>
        /// 当前选中的玩家
        /// </summary>
        public UserGameRole? SelectedUserGameRole
        {
            get => selectedUserGameRole;

            set => SetPropertyAndCallbackOnCompletion(ref selectedUserGameRole, value, UpdateAvatarListAsync);
        }

        /// <summary>
        /// 选中的全武器
        /// </summary>
        public Weapon? SelectedFullWeapon
        {
            get => selectedFullWeapon;
            set => SetPropertyAndCallbackOnCompletion(ref selectedFullWeapon, value, UpdateFullWeaponAvatarDetailData);
        }

        /// <summary>
        /// 打开界面时触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        /// <summary>
        /// 计算命令
        /// </summary>
        public ICommand ComputeCommand { get; }

        /// <summary>
        /// 全角色材料添加命令
        /// </summary>
        public ICommand AddFullCharacterMaterialCommand { get; }

        /// <summary>
        /// 全角色计算命令
        /// </summary>
        public ICommand FullAvatarComputeCommand { get; }

        /// <summary>
        /// 全武器计算命令
        /// </summary>
        public ICommand FullWeaponComputeCommand { get; }

        /// <summary>
        /// 全武器添加材料命令
        /// </summary>
        public ICommand AddFullWeaponMaterialCommand { get; }

        /// <summary>
        /// 添加角色材料命令
        /// </summary>
        public ICommand AddCharacterMaterialCommand { get; }

        /// <summary>
        /// 添加武器材料命令
        /// </summary>
        public ICommand AddWeaponMaterialCommand { get; }

        /// <summary>
        /// 退出界面时触发的命令
        /// </summary>
        public ICommand CloseUICommand { get; }

        /// <summary>
        /// 修改材料命令
        /// </summary>
        private ICommand EditMaterialCommand { get; }

        /// <summary>
        /// 移除材料命令
        /// </summary>
        private ICommand RemoveMaterialCommand { get; }

        private async Task OpenUIAsync()
        {
            try
            {
                MaterialList = materialListService.Load(EditMaterialCommand, RemoveMaterialCommand);
                IsListEmpty = MaterialList.IsNullOrEmpty();

                UserGameRoles = await userGameRoleProvider.GetUserGameRolesAsync(CancellationToken);
                SelectedUserGameRole = UserGameRoles.FirstOrDefault();

                List<Avatar> avatars = await calculator.GetAvatarListAsync(new(), cancellationToken: CancellationToken);
                FullAvatars = avatars
                    .Where(x => x.Name != "旅行者")
                    .ToList();
                SelectedFullAvatar = FullAvatars.FirstOrDefault();

                FullWeapons = await calculator.GetWeaponListAsync(new(), cancellationToken: CancellationToken);
                SelectedFullWeapon = FullWeapons.FirstOrDefault();
            }
            catch (TaskCanceledException)
            {
                this.Log("Open UI cancelled");
            }
        }

        private void CloseUI()
        {
            materialListService.Save(MaterialList);
        }

        private async Task ComputeAsync()
        {
            try
            {
                if (SelectedAvatar is not null && AvatarDetailData is not null)
                {
                    AvatarPromotionDelta delta = new()
                    {
                        AvatarId = SelectedAvatar.Id,
                        AvatarLevelCurrent = SelectedAvatar.LevelCurrent,
                        AvatarLevelTarget = SelectedAvatar.LevelTarget,
                        SkillList = AvatarDetailData.SkillList?.Select(s => s.ToPromotionDelta()),
                        Weapon = AvatarDetailData.Weapon?.ToPromotionDelta(),
                        ReliquaryList = AvatarDetailData.ReliquaryList?.Select(r => r.ToPromotionDelta()),
                    };
                    Consumption = await calculator.ComputeAsync(delta, CancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                this.Log("ComputeAsync canceled by user switch page");
            }
        }

        private async Task ComputerFullAvatarAsync()
        {
            try
            {
                if (SelectedFullAvatar is not null && FullAvatarDetailData is not null)
                {
                    AvatarPromotionDelta delta = new()
                    {
                        AvatarId = SelectedFullAvatar.Id,
                        AvatarLevelCurrent = SelectedFullAvatar.LevelCurrent,
                        AvatarLevelTarget = SelectedFullAvatar.LevelTarget,
                        SkillList = FullAvatarDetailData.SkillList?.Select(s => s.ToPromotionDelta()),
                    };
                    FullAvatarConsumption = await calculator.ComputeAsync(delta, CancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                this.Log("ComputerFullAvatarAsync canceled by user switch page");
            }
        }

        private async Task ComputerFullWeaponAsync()
        {
            try
            {
                if (FullWeaponAvatarDetailData is not null)
                {
                    AvatarPromotionDelta delta = new()
                    {
                        Weapon = FullWeaponAvatarDetailData.Weapon?.ToPromotionDelta(),
                    };
                    FullWeaponConsumption = await calculator.ComputeAsync(delta, CancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                this.Log("ComputerFullWeaponAsync canceled by user switch page");
            }
        }

        private async Task AddCharacterMaterialToListAsync(string? option)
        {
            Calculable calculable = Must.NotNull(SelectedAvatar!);

            List<ConsumeItem> items = option switch
            {
                AvatarTag => Must.NotNull(Consumption?.AvatarConsume!),
                SkillTag => Must.NotNull(Consumption?.AvatarSkillConsume!),
                _ => throw Must.NeverHappen(),
            };

            string category = option switch
            {
                AvatarTag => "角色消耗",
                SkillTag => "天赋消耗",
                _ => throw Must.NeverHappen(),
            };

            if (await ConfirmAddAsync(calculable.Name!, category))
            {
                MaterialList?.Insert(0, new(calculable, items, EditMaterialCommand, RemoveMaterialCommand));
                IsListEmpty = MaterialList.IsNullOrEmpty();
            }
        }

        private async Task AddFullCharacterMaterialToListAsync(string? option)
        {
            Calculable calculable = Must.NotNull(SelectedFullAvatar!);

            List<ConsumeItem> items = option switch
            {
                AvatarTag => Must.NotNull(FullAvatarConsumption?.AvatarConsume!),
                SkillTag => Must.NotNull(FullAvatarConsumption?.AvatarSkillConsume!),
                _ => throw Must.NeverHappen(),
            };

            string category = option switch
            {
                AvatarTag => "角色消耗",
                SkillTag => "天赋消耗",
                _ => throw Must.NeverHappen(),
            };

            if (await ConfirmAddAsync(calculable.Name!, category))
            {
                MaterialList?.Insert(0, new(calculable, items, EditMaterialCommand, RemoveMaterialCommand));
                IsListEmpty = MaterialList.IsNullOrEmpty();
            }
        }

        private async Task AddWeaponMaterialToListAsync()
        {
            Calculable calculable = Must.NotNull(AvatarDetailData?.Weapon!);
            List<ConsumeItem> items = Must.NotNull(Consumption?.WeaponConsume!);

            if (await ConfirmAddAsync(calculable.Name!, "武器消耗"))
            {
                MaterialList?.Insert(0, new(calculable, items, EditMaterialCommand, RemoveMaterialCommand));
                IsListEmpty = MaterialList.IsNullOrEmpty();
            }
        }

        private async Task AddFullWeaponMaterialToListAsync()
        {
            Calculable calculable = Must.NotNull(FullWeaponAvatarDetailData?.Weapon!);
            List<ConsumeItem> items = Must.NotNull(FullWeaponConsumption?.WeaponConsume!);

            if (await ConfirmAddAsync(calculable.Name!, "武器消耗"))
            {
                MaterialList?.Insert(0, new(calculable, items, EditMaterialCommand, RemoveMaterialCommand));
                IsListEmpty = MaterialList.IsNullOrEmpty();
            }
        }

        private async Task RemoveMaterialFromListAsync(CalculableConsume? item)
        {
            ContentDialogResult result = await new ContentDialog()
            {
                Title = $"从清单中删除？",
                Content = "该操作不可撤销",
                PrimaryButtonText = "确认",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Close,
            }.ShowAsync();

            if (result == ContentDialogResult.Primary && item is not null)
            {
                MaterialList?.Remove(item);
                IsListEmpty = MaterialList.IsNullOrEmpty();
            }
        }

        private async Task EditMaterialAsync(CalculableConsume? item)
        {
            if (item is not null)
            {
                await new MaterialListDialog(item).ShowAsync();

                // remove 0 count item
                foreach (ConsumeItem cItem in item.ConsumeItems.ToList())
                {
                    if (cItem.Num <= 0)
                    {
                        item.ConsumeItems.Remove(cItem);
                    }
                }
            }
        }

        private async Task<bool> ConfirmAddAsync(string name, string category)
        {
            ContentDialogResult result = await new ContentDialog()
            {
                Title = $"添加 {name} 的 {category} 到清单？",
                Content = "稍后可以切换到材料清单查看",
                PrimaryButtonText = "确认",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
            }.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        [PropertyChangedCallback]
        private async Task UpdateAvatarListAsync(UserGameRole? selected)
        {
            try
            {
                if (selected is not null)
                {
                    Must.NotNull(selected.GameUid!);
                    Must.NotNull(selected.Region!);

                    List<Avatar> avatars = await calculator.GetSyncedAvatarListAsync(
                        new(selected.GameUid, selected.Region),
                        true,
                        CancellationToken);

                    Avatars = avatars
                        .Where(x => x.Name != "旅行者")
                        .ToList();
                    SelectedAvatar = Avatars?.FirstOrDefault();
                }
            }
            catch (TaskCanceledException)
            {
                this.Log("UpdateAvatarList canceled by user switch page");
            }
        }

        [PropertyChangedCallback]
        private async Task UpdateAvatarDetailDataAsync(Avatar? avatar)
        {
            try
            {
                Consumption = null;

                if (avatar is not null)
                {
                    Must.NotNull(SelectedUserGameRole!);

                    string uid = Must.NotNull(SelectedUserGameRole.GameUid!);
                    string region = Must.NotNull(SelectedUserGameRole.Region!);

                    int avatarId = avatar.Id;

                    AvatarDetailData = await calculator.GetSyncedAvatarDetailDataAsync(avatarId, uid, region, CancellationToken);

                    avatar.LevelTarget = avatar.LevelCurrent;
                    AvatarDetailData?.SkillList?.ForEach(x => x.LevelTarget = x.LevelCurrent);
                    if (AvatarDetailData?.Weapon is not null)
                    {
                        AvatarDetailData.Weapon.LevelTarget = AvatarDetailData.Weapon.LevelCurrent;
                    }

                    AvatarDetailData?.ReliquaryList?.ForEach(x => x.LevelTarget = x.LevelCurrent);
                }
            }
            catch (TaskCanceledException)
            {
                this.Log("UpdateAvatarDetailData canceled");
            }
        }

        [PropertyChangedCallback]
        [SuppressMessage("", "VSTHRD100")]
        private async void UpdateFullAvatarDetailData(Avatar? avatar)
        {
            try
            {
                FullAvatarConsumption = null;
                if (avatar is not null)
                {
                    FullAvatarDetailData = new()
                    {
                        SkillList = await calculator.GetAvatarSkillListAsync(avatar, CancellationToken),
                    };
                    avatar.LevelTarget = 90;
                    FullAvatarDetailData.SkillList.ForEach(x => x.LevelCurrent = 1);
                    FullAvatarDetailData.SkillList.ForEach(x => x.LevelTarget = 10);
                }
            }
            catch (TaskCanceledException)
            {
                this.Log("UpdateFullAvatarDetailData canceled");
            }
        }

        [PropertyChangedCallback]
        private void UpdateFullWeaponAvatarDetailData()
        {
            FullWeaponConsumption = null;
            FullWeaponAvatarDetailData = new() { Weapon = SelectedFullWeapon };

            if (SelectedFullWeapon is not null && FullWeaponAvatarDetailData?.Weapon is not null)
            {
                FullWeaponAvatarDetailData.Weapon.LevelCurrent = 1;
                FullWeaponAvatarDetailData.Weapon.LevelTarget = SelectedFullWeapon.MaxLevel;
            }
        }
    }
}