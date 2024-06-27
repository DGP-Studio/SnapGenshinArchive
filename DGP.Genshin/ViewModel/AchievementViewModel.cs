using CommunityToolkit.Mvvm.Input;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.DataModel.Achievement;
using DGP.Genshin.DataModel.Achievement.Decomposed;
using DGP.Genshin.Service.Abstraction.Achievement;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Core.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 成就视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    internal class AchievementViewModel : ObservableObject2
    {
        private readonly MetadataViewModel metadataViewModel;
        private readonly IAchievementService achievementService;
        private ObservableCollection<Achievement>? achievements;
        private AchievementGoal? selectedAchievementGoal;
        private List<AchievementGoal> achievementGoals = null!;

        private string? query;
        private bool incompletedFirst = true;
        private double completedProgress;

        /// <summary>
        /// 构造一个新的成就视图模型
        /// </summary>
        /// <param name="achievementService">成就服务</param>
        /// <param name="metadataViewModel">元数据视图模型</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        public AchievementViewModel(IAchievementService achievementService, MetadataViewModel metadataViewModel)
        {
            this.metadataViewModel = metadataViewModel;
            this.achievementService = achievementService;
            OpenUICommand = new RelayCommand<Func<IAchievementService, IEnumerable<IdTime>?>>(OpenUI);
            CloseUICommand = new RelayCommand(CloseUI);
            RefreshQueryCommand = new RelayCommand<string>(RefreshQuery);
            ImportFromCocoGoatCommand = new RelayCommand(ImportFromCocoGoat);
            ImportFromClipboardCommand = new RelayCommand(ImportFromClipBoard);
        }

        /// <summary>
        /// 成就大纲
        /// </summary>
        public List<AchievementGoal> AchievementGoals
        {
            get => achievementGoals;

            set => SetProperty(ref achievementGoals, value);
        }

        /// <summary>
        /// 当前的成就大纲页
        /// </summary>
        public AchievementGoal? SelectedAchievementGoal
        {
            get => selectedAchievementGoal;

            set => SetPropertyAndCallbackOnCompletion(ref selectedAchievementGoal, value, RefreshView);
        }

        /// <summary>
        /// 成就列表
        /// </summary>
        public ObservableCollection<Achievement>? Achievements
        {
            get => achievements;

            set => SetProperty(ref achievements, value);
        }

        /// <summary>
        /// 未完成优先
        /// </summary>
        public bool IncompletedFirst
        {
            get => incompletedFirst;
            set => SetPropertyAndCallbackOnCompletion(ref incompletedFirst, value, RefreshView);
        }

        /// <summary>
        /// 当前成就分类完成进度
        /// </summary>
        public double CompletedProgress
        {
            get => completedProgress;
            set => SetProperty(ref completedProgress, value);
        }

        /// <summary>
        /// 打开界面时触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        /// <summary>
        /// 关闭界面时触发的命令
        /// </summary>
        public ICommand CloseUICommand { get; }

        /// <summary>
        /// 筛选查询命令
        /// </summary>
        public ICommand RefreshQueryCommand { get; }

        /// <summary>
        /// 从椰羊导入命令
        /// </summary>
        public ICommand ImportFromCocoGoatCommand { get; }

        /// <summary>
        /// 从剪贴板导入
        /// </summary>
        public ICommand ImportFromClipboardCommand { get; set; }

        private void OpenUI(Func<IAchievementService, IEnumerable<IdTime>?>? importer)
        {
            AchievementGoals = metadataViewModel.AchievementGoals;
            Achievements = new(metadataViewModel.Achievements);

            SetAchievementsState(achievementService.GetCompletedItems(), Achievements);
            SelectedAchievementGoal = AchievementGoals.First();
            CollectionViewSource.GetDefaultView(Achievements).Filter = OnFilterAchievement;

            // has a specific importer.
            if (importer != null)
            {
                IEnumerable<IdTime>? result = importer.Invoke(achievementService);
                ImportData(result);
            }
        }

        private void CloseUI()
        {
            if (Achievements is not null)
            {
                achievementService.SaveItems(Achievements);
            }
        }

        private void ImportFromCocoGoat()
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
                IEnumerable<IdTime>? data = achievementService.TryGetImportData(ImportAchievementSource.Cocogoat, openFileDialog.FileName);
                ImportData(data);
            }
        }

        private void ImportFromClipBoard()
        {
            string dataString = Clipboard.GetText();
            IEnumerable<IdTime>? data = achievementService.TryGetImportData(dataString);
            ImportData(data);
        }

        private void ImportData(IEnumerable<IdTime>? data)
        {
            if (data != null)
            {
                Must.NotNull(Achievements!);
                int totalCount = SetAchievementsState(data, Achievements);
                this.Log(totalCount);
                RefreshView();
                int left = data.Count() - totalCount;

                new ToastContentBuilder()
                    .AddText("导入成功")
                    .AddText($"共同步了 {totalCount} 个成就。{left} 个成就导入失败。")
                    .SafeShow();
            }
            else
            {
                new ToastContentBuilder()
                    .AddText("导入失败")
                    .AddText($"导入的数据中包含了不正确的项目。")
                    .SafeShow();
            }
        }

        private int SetAchievementsState(IEnumerable<IdTime> data, ObservableCollection<Achievement> achievements)
        {
            Dictionary<int, Achievement> mappedAchievements = achievements.ToDictionary(a => a.Id);
            int count = 0;

            // load completed item
            foreach (IdTime item in data)
            {
                if (mappedAchievements.TryGetValue(item.Id,out Achievement? achievement))
                {
                    achievement.CompleteDateTime = item.Time;
                    achievement.IsCompleted = true;
                    count++;
                }
            }

            // load decomposed step
            foreach (DecomposedAchievement decomposed in metadataViewModel.DecomposedAchievements)
            {
                Achievement achievement = mappedAchievements[decomposed.AchievementId];
                achievement.Decomposition = decomposed;

                // initialize steps
                achievement.Decomposition.Steps = achievement.Decomposition.Decomposed!
                    .Select(d => new DecomposedStep() { Description = d })
                    .ToList();
            }

            // load completed step
            foreach (IdSteps step in achievementService.GetCompletedSteps())
            {
                Achievement achievement = mappedAchievements[step.Id];

                for (int i = 0; i < (step.Steps!).Count; i++)
                {
                    achievement.Decomposition!.Steps![i].IsCompleted = step.Steps[i];
                }
            }

            return count;
        }

        [PropertyChangedCallback]
        private void RefreshView()
        {
            if (SelectedAchievementGoal != null && Achievements != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(Achievements);
                view.SortDescriptions.Clear();
                if (IncompletedFirst)
                {
                    view.SortDescriptions.Add(new(nameof(Achievement.IsCompleted), ListSortDirection.Ascending));
                }

                view.Refresh();

                // update progress value
                int total = 0;
                int completed = 0;

                foreach (Achievement item in view)
                {
                    total++;
                    if (item.IsCompleted)
                    {
                        completed++;
                    }
                }

                CompletedProgress = (double)completed / total;
            }
        }

        private void RefreshQuery(string? query = null)
        {
            // prevent duplecate query
            if (this.query != query)
            {
                this.query = query;
                RefreshView();
            }
        }

        private bool OnFilterAchievement(object obj)
        {
            if (obj is Achievement achi)
            {
                bool goalMatch = achi.GoalId == SelectedAchievementGoal!.Id;

                bool queryMatch = true;
                if (!string.IsNullOrWhiteSpace(query))
                {
                    queryMatch = achi.Title!.Contains(query) || achi.Description!.Contains(query);
                }

                return goalMatch && queryMatch;
            }

            return false;
        }
    }
}