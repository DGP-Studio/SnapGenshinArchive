using CommunityToolkit.Mvvm.Input;
using DGP.Genshin.DataModel.GachaStatistic;
using DGP.Genshin.DataModel.GachaStatistic.Banner;
using DGP.Genshin.Factory.Abstraction;
using DGP.Genshin.MiHoYoAPI.Gacha;
using DGP.Genshin.Service.Abstraction.GachaStatistic;
using DGP.Genshin.Service.GachaStatistic;
using Microsoft.Win32;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Core.Mvvm;
using Snap.Data.Utility;
using Snap.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 祈愿记录视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class GachaStatisticViewModel : ObservableObject2
    {
        private readonly IGachaStatisticService gachaStatisticService;
        private readonly TaskPreventer taskPreventer = new();

        private Statistic? statistic;
        private UidGachaData? selectedUserGachaData;
        private FetchProgress? fetchProgress;
        private SpecificBanner? selectedSpecificBanner;
        private GachaDataCollection userGachaDataCollection = new();
        private bool isFullFetch;

        /// <summary>
        /// 构造一个新的祈愿记录视图模型
        /// </summary>
        /// <param name="gachaStatisticService">祈愿记录服务</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        public GachaStatisticViewModel(IGachaStatisticService gachaStatisticService, IAsyncRelayCommandFactory asyncRelayCommandFactory)
        {
            this.gachaStatisticService = gachaStatisticService;

            Progress = new Progress<FetchProgress>(OnFetchProgressed);

            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
            GachaLogAutoFindCommand = asyncRelayCommandFactory.Create(RefreshByAutoFindModeAsync);
            GachaLogManualCommand = asyncRelayCommandFactory.Create(RefreshByManualAsync);
            ImportFromUIGFJCommand = asyncRelayCommandFactory.Create(ImportFromUIGFJAsync);
            ImportFromUIGFWCommand = asyncRelayCommandFactory.Create(ImportFromUIGFWAsync);
            ExportToUIGFWCommand = asyncRelayCommandFactory.Create(ExportToUIGFWAsync);
            ExportToUIGFJCommand = asyncRelayCommandFactory.Create(ExportToUIGFJAsync);
            OpenGachaStatisticFolderCommand = new RelayCommand(OpenGachaStatisticFolder);
        }

        /// <summary>
        /// 当前的统计信息
        /// </summary>
        public Statistic? Statistic
        {
            get => statistic;

            set => SetProperty(ref statistic, value);
        }

        /// <summary>
        /// 当前选择的UID
        /// </summary>
        public UidGachaData? SelectedUserGachaData
        {
            get => selectedUserGachaData;

            set => SetPropertyAndCallbackOverridePropertyState(ref selectedUserGachaData, value, SyncStatisticWithUid);
        }

        /// <summary>
        /// 所有UID
        /// </summary>
        public GachaDataCollection UserGachaDataCollection
        {
            get => userGachaDataCollection;

            set => SetProperty(ref userGachaDataCollection, value);
        }

        /// <summary>
        /// 当前的获取进度
        /// </summary>
        public FetchProgress? FetchProgress
        {
            get => fetchProgress;

            set => SetProperty(ref fetchProgress, value);
        }

        /// <summary>
        /// 选定的特定池
        /// </summary>
        public SpecificBanner? SelectedSpecificBanner
        {
            get => selectedSpecificBanner;

            set => SetProperty(ref selectedSpecificBanner, value);
        }

        /// <summary>
        /// 是否全量刷新
        /// </summary>
        public bool IsFullFetch
        {
            get => isFullFetch;

            set => SetProperty(ref isFullFetch, value);
        }

        /// <summary>
        /// 打开界面触发命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        /// <summary>
        /// 自动查找Url模式命令
        /// </summary>
        public ICommand GachaLogAutoFindCommand { get; }

        /// <summary>
        /// 手动输入Url模式命令
        /// </summary>
        public ICommand GachaLogManualCommand { get; }

        /// <summary>
        /// 从UIGFJ导入
        /// </summary>
        public ICommand ImportFromUIGFJCommand { get; }

        /// <summary>
        /// 导出到UIGFJ
        /// </summary>
        public ICommand ExportToUIGFJCommand { get; }

        /// <summary>
        /// 从UIGFW导入
        /// </summary>
        public ICommand ImportFromUIGFWCommand { get; }

        /// <summary>
        /// 导出到UIGFW
        /// </summary>
        public ICommand ExportToUIGFWCommand { get; }

        /// <summary>
        /// 打开祈愿记录数据文件夹
        /// </summary>
        public ICommand OpenGachaStatisticFolderCommand { get; }

        private IProgress<FetchProgress> Progress { get; }

        private async Task OpenUIAsync()
        {
            await gachaStatisticService.LoadLocalGachaDataAsync(UserGachaDataCollection);
            SelectedUserGachaData = UserGachaDataCollection.FirstOrDefault();
        }

        private async Task RefreshByAutoFindModeAsync()
        {
            if (taskPreventer.ShouldExecute)
            {
                try
                {
                    (bool isOk, string uid) = await gachaStatisticService.RefreshAsync(UserGachaDataCollection, GachaLogUrlMode.Proxy, Progress, IsFullFetch);
                    FetchProgress = null;
                    if (isOk)
                    {
                        SelectedUserGachaData = UserGachaDataCollection.FirstOrDefault(u => u.Uid == uid);
                    }
                }
                catch (Exception ex)
                {
                    this.Log(ex);
                }

                taskPreventer.Release();
            }
        }

        private async Task RefreshByManualAsync()
        {
            if (taskPreventer.ShouldExecute)
            {
                try
                {
                    (bool isOk, string uid) = await gachaStatisticService.RefreshAsync(UserGachaDataCollection, GachaLogUrlMode.ManualInput, Progress, IsFullFetch);
                    FetchProgress = null;
                    if (isOk)
                    {
                        SelectedUserGachaData = UserGachaDataCollection.FirstOrDefault(u => u.Uid == uid);
                    }
                }
                catch (Exception ex)
                {
                    this.Log(ex);
                }

                taskPreventer.Release();
            }
        }

        private void OnFetchProgressed(FetchProgress p)
        {
            FetchProgress = p;
        }

        private async Task ImportFromUIGFJAsync()
        {
            if (taskPreventer.ShouldExecute)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "JS对象简谱文件|*.json",
                    Title = "从 Json 文件导入",
                    Multiselect = false,
                    CheckFileExists = true,
                };
                if (openFileDialog.ShowDialog() is true)
                {
                    (bool isOk, string uid) = await gachaStatisticService.ImportFromUIGFJAsync(UserGachaDataCollection, openFileDialog.FileName);
                    if (!isOk)
                    {
                        await new ContentDialog()
                        {
                            Title = "导入祈愿记录失败",
                            Content = "文件不是UIGF格式，或支持的UIGF版本较低",
                            PrimaryButtonText = "确定",
                            DefaultButton = ContentDialogButton.Primary,
                        }.ShowAsync();
                    }
                    else
                    {
                        SelectedUserGachaData = UserGachaDataCollection.FirstOrDefault(u => u.Uid == uid);
                    }
                }

                taskPreventer.Release();
            }
        }

        private async Task ExportToUIGFJAsync()
        {
            if (taskPreventer.ShouldExecute)
            {
                if (SelectedUserGachaData is not null)
                {
                    SaveFileDialog dialog = new()
                    {
                        Filter = "JS对象简谱文件|*.json",
                        Title = "保存到文件",
                        ValidateNames = true,
                        CheckPathExists = true,
                        FileName = $"{SelectedUserGachaData.Uid}.json",
                    };
                    if (dialog.ShowDialog() is true)
                    {
                        await gachaStatisticService.ExportDataToJsonAsync(UserGachaDataCollection, SelectedUserGachaData.Uid, dialog.FileName);
                        await new ContentDialog
                        {
                            Title = "导出祈愿记录完成",
                            Content = $"祈愿记录已导出至 {dialog.SafeFileName}",
                            PrimaryButtonText = "确定",
                            DefaultButton = ContentDialogButton.Primary,
                        }.ShowAsync();
                    }

                    taskPreventer.Release();
                }
            }
        }

        private async Task ImportFromUIGFWAsync()
        {
            if (taskPreventer.ShouldExecute)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Excel 工作簿|*.xlsx",
                    Title = "从 Excel 文件导入",
                    Multiselect = false,
                    CheckFileExists = true,
                };
                if (openFileDialog.ShowDialog() is true)
                {
                    (bool isOk, string uid) = await gachaStatisticService.ImportFromUIGFWAsync(UserGachaDataCollection, openFileDialog.FileName);
                    if (!isOk)
                    {
                        await new ContentDialog()
                        {
                            Title = "导入失败",
                            Content = "文件不是UIGF格式，或支持的UIGF版本较低",
                            PrimaryButtonText = "确定",
                            DefaultButton = ContentDialogButton.Primary,
                        }.ShowAsync();
                    }
                    else
                    {
                        SelectedUserGachaData = UserGachaDataCollection.FirstOrDefault(u => u.Uid == uid);
                    }
                }

                taskPreventer.Release();
            }
        }

        private async Task ExportToUIGFWAsync()
        {
            if (taskPreventer.ShouldExecute)
            {
                if (SelectedUserGachaData is not null)
                {
                    SaveFileDialog dialog = new()
                    {
                        Filter = "Excel 工作簿|*.xlsx",
                        Title = "保存到表格",
                        ValidateNames = true,
                        CheckPathExists = true,
                        FileName = $"{SelectedUserGachaData.Uid}.xlsx",
                    };
                    if (dialog.ShowDialog() is true)
                    {
                        this.Log("try to export to excel");
                        await gachaStatisticService.ExportDataToExcelAsync(UserGachaDataCollection, SelectedUserGachaData.Uid, dialog.FileName);
                        await new ContentDialog
                        {
                            Title = "导出祈愿记录完成",
                            Content = $"祈愿记录已导出至 {dialog.SafeFileName}",
                            PrimaryButtonText = "确定",
                            DefaultButton = ContentDialogButton.Primary,
                        }.ShowAsync();
                    }

                    taskPreventer.Release();
                }
            }
        }

        private void OpenGachaStatisticFolder()
        {
            FileExplorer.Open(PathContext.Locate("GachaStatistic"));
        }

        /// <summary>
        /// 同步统计数据与当前uid
        /// </summary>
        [PropertyChangedCallback]
        private void SyncStatisticWithUid()
        {
            this.Log($"try read:{SelectedUserGachaData}");
            if (SelectedUserGachaData is not null)
            {
                Statistic = gachaStatisticService.GetStatistic(UserGachaDataCollection, SelectedUserGachaData.Uid);
                SelectedSpecificBanner = Statistic.SpecificBanners?.FirstOrDefault();
            }
        }
    }
}