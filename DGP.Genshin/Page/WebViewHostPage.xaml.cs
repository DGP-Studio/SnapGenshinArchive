using DGP.Genshin.Control;
using DGP.Genshin.Core.Notification;
using DGP.Genshin.DataModel.WebViewLobby;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.Threading;
using Microsoft.Web.WebView2.Core;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 自定义网页呈现页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class WebViewHostPage : ModernWpf.Controls.Page
    {
        private WebViewEntry? entry;

        /// <summary>
        /// 构造一个新的自定义网页呈现页面
        /// </summary>
        public WebViewHostPage()
        {
            if (WebView2Helper.IsSupported)
            {
                InitializeComponent();
            }
            else
            {
                new WebView2RuntimeWindow().ShowDialog();
                Verify.FailOperation("未找到可用的 WebView2运行时 安装");
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            entry = e.ExtraData as WebViewEntry;
            base.OnNavigatedTo(e);
        }

        /// <inheritdoc/>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WebView?.Dispose();
            base.OnNavigatedFrom(e);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            OpenTargetUrlAsync().Forget();
        }

        private void WebViewNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            ExecuteJavaScriptAsync().Forget();
        }

        private async Task OpenTargetUrlAsync()
        {
            try
            {
                await WebView.EnsureCoreWebView2Async();
                WebView.CoreWebView2.ProcessFailed += (s, e) => WebView?.Dispose();
            }
            catch
            {
                return;
            }

            if (entry is not null)
            {
                try
                {
                    WebView.CoreWebView2.Navigate(entry.NavigateUrl);
                }
                catch
                {
                    new ToastContentBuilder()
                    .AddText("无法导航到指定的网页")
                    .SafeShow();
                }
            }
        }

        private async Task ExecuteJavaScriptAsync()
        {
            if (entry?.JavaScript is not null)
            {
                this.Log("开始执行脚本");
                string? result = await WebView.CoreWebView2.ExecuteScriptAsync(entry.JavaScript);
                this.Log($"执行完成:{result}");
            }
        }
    }
}