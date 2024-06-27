using Microsoft.VisualStudio.Threading;
using Microsoft.Web.WebView2.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DGP.Genshin.Control.Cookie
{
    /// <summary>
    /// CookieWindow.xaml 的交互逻辑
    /// </summary>
    public sealed partial class CookieWindow : Window, IDisposable
    {
        /// <summary>
        /// 构造一个新的Cookie窗体
        /// </summary>
        public CookieWindow()
        {
            InitializeComponent();
            WebView.CoreWebView2InitializationCompleted += WebViewCoreWebView2InitializationCompleted;
        }

        /// <summary>
        /// Cookie
        /// </summary>
        public string? Cookie { get; private set; }

        /// <summary>
        /// 是否登录成功
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            WebView?.Dispose();
        }

        private void WebViewCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            WebView.CoreWebView2.ProcessFailed += WebViewCoreWebView2ProcessFailed;
            if (e.IsSuccess)
            {
                ContinueButton.IsEnabled = true;
            }
        }

        private void WebViewCoreWebView2ProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
        {
            ContinueButton.IsEnabled = false;
            WebView?.Dispose();
        }

        private void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            ContinueAsync().Forget();
        }

        private async Task ContinueAsync()
        {
            List<CoreWebView2Cookie> cookies = await WebView.CoreWebView2.CookieManager.GetCookiesAsync("https://bbs.mihoyo.com");
            string[] cookiesString = cookies.Select(c => $"{c.Name}={c.Value};").ToArray();
            Cookie = string.Concat(cookiesString);
            if (Cookie.Contains("account_id"))
            {
                IsLoggedIn = true;
                Close();
            }
        }
    }
}