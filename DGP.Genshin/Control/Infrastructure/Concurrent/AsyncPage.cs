using System.Threading;
using System.Windows.Navigation;

namespace DGP.Genshin.Control.Infrastructure.Concurrent
{
    /// <summary>
    /// 表示支持取消加载的异步页面
    /// 在被导航到其他页面前触发取消异步通知
    /// </summary>
    public abstract class AsyncPage : ModernWpf.Controls.Page
    {
        private readonly CancellationTokenSource viewLoadingConcellationTokenSource = new();

        /// <summary>
        /// 自动设置页面的数据上下文
        /// </summary>
        /// <param name="viewModel">视图模型</param>
        public AsyncPage(ISupportCancellation viewModel)
        {
            viewModel.CancellationToken = viewLoadingConcellationTokenSource.Token;
            DataContext = viewModel;
        }

        /// <inheritdoc/>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            viewLoadingConcellationTokenSource.Cancel();
            base.OnNavigatingFrom(e);
        }
    }
}