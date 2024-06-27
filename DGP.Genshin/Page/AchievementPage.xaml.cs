using DGP.Genshin.ViewModel;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;
using System.Windows.Navigation;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 成就页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class AchievementPage : ModernWpf.Controls.Page
    {
        /// <summary>
        /// 构造一个新的成就页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public AchievementPage(AchievementViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((AchievementViewModel)DataContext).OpenUICommand.Execute(e.ExtraData);
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            ((AchievementViewModel)DataContext).CloseUICommand.Execute(null);
        }

        private void AutoSuggestBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ((AchievementViewModel)DataContext).RefreshQueryCommand.Execute(sender.Text);
        }

        private void AutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // 仅空白时更新筛选
            if (string.IsNullOrWhiteSpace(sender.Text))
            {
                ((AchievementViewModel)DataContext).RefreshQueryCommand.Execute(sender.Text);
            }
        }
    }
}