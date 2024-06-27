using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using ModernWpf.Controls;
using Snap.Core.DependencyInjection;
using WPFUI.Controls;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 玩家查询页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class RecordPage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的玩家查询页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public RecordPage(RecordViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }

        private RecordViewModel ViewModel
        {
            get => (RecordViewModel)DataContext;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string? uid = args.ChosenSuggestion != null ? args.ChosenSuggestion.ToString() : args.QueryText;
            ViewModel.QueryCommand.Execute(uid);
        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            string? uid = (string?)((CardAction)sender).CommandParameter;
            ViewModel.QueryCommand.Execute(uid);
        }
    }
}