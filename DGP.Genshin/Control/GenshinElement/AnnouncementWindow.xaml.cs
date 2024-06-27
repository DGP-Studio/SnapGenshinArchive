using Microsoft.VisualStudio.Threading;
using Snap.Data.Utility;
using System.Threading.Tasks;

namespace DGP.Genshin.Control.GenshinElement
{
    /// <summary>
    /// 原神公告窗口
    /// </summary>
    public sealed partial class AnnouncementWindow : Window, IDisposable
    {
        private const string MihoyoSDKDefinition =
            "window.miHoYoGameJSSDK = {" +
            "openInBrowser: function(url){ window.chrome.webview.postMessage(url); }," +
            "openInWebview: function(url){ location.href = url }}";

        private readonly string? targetContent;

        /// <summary>
        /// 构造一个新的公告窗体
        /// </summary>
        /// <param name="content">要展示的内容</param>
        public AnnouncementWindow(string? content)
        {
            // 不需要在此处检查WebView2可用性，由使用方代为检查
            targetContent = content;
            InitializeComponent();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            WebView?.Dispose();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadAnnouncementAsync().Forget();
        }

        private async Task LoadAnnouncementAsync()
        {
            try
            {
                await WebView.EnsureCoreWebView2Async();
                WebView.CoreWebView2.ProcessFailed += (_, _) => WebView?.Dispose();

                // support click open browser.
                await WebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(MihoyoSDKDefinition);
                WebView.CoreWebView2.WebMessageReceived += (_, e) => Browser.Open(e.TryGetWebMessageAsString);
            }
            catch
            {
                Close();
                return;
            }

            WebView.NavigateToString(targetContent);
        }
    }
}