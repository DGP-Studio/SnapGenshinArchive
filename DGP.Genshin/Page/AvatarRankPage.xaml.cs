using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 角色评分页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class AvatarRankPage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的角色评分页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public AvatarRankPage(AvatarRankViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }

        private AvatarRankViewModel ViewModel
        {
            get => (AvatarRankViewModel)DataContext;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string? uid = args.ChosenSuggestion != null ? args.ChosenSuggestion.ToString() : args.QueryText;
            ViewModel.QueryCommand.Execute(uid);
        }
    }
}