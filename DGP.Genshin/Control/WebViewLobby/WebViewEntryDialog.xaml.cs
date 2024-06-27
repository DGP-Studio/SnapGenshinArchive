using DGP.Genshin.DataModel.WebViewLobby;
using Microsoft.VisualStudio.Threading;
using ModernWpf.Controls;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DGP.Genshin.Control.WebViewLobby
{
    /// <summary>
    /// 自定义网页对话框
    /// </summary>
    public partial class WebViewEntryDialog : ContentDialog
    {
        private const string WhiteSpace = " ";
        private static readonly DependencyProperty NavigateUrlProperty = Property<WebViewEntryDialog>.Depend<string>(nameof(NavigateUrl));
        private static readonly DependencyProperty EntryNameProperty = Property<WebViewEntryDialog>.Depend<string>(nameof(EntryName));
        private static readonly DependencyProperty IconUrlProperty = Property<WebViewEntryDialog>.Depend<string>(nameof(IconUrl));
        private static readonly DependencyProperty JavaScriptProperty = Property<WebViewEntryDialog>.Depend<string>(nameof(JavaScript));
        private static readonly DependencyProperty ShowInNavViewProperty = Property<WebViewEntryDialog>.Depend(nameof(ShowInNavView), true);

        /// <summary>
        /// 构造一个新的自定义网页对话框
        /// </summary>
        /// <param name="entry">编辑的自定义网页入口</param>
        public WebViewEntryDialog(WebViewEntry? entry = null)
        {
            if (entry is not null)
            {
                NavigateUrl = entry.NavigateUrl;
                EntryName = entry.Name;
                IconUrl = entry.IconUrl;
                JavaScript = entry.JavaScript;
                ShowInNavView = entry.ShowInNavView;
            }

            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// 导航Url
        /// </summary>
        public string NavigateUrl
        {
            get => (string)GetValue(NavigateUrlProperty);

            set => SetValue(NavigateUrlProperty, value);
        }

        /// <summary>
        /// 入口名称
        /// </summary>
        public string EntryName
        {
            get => (string)GetValue(EntryNameProperty);

            set => SetValue(EntryNameProperty, value);
        }

        /// <summary>
        /// 图标Url
        /// </summary>
        [AllowNull]
        public string IconUrl
        {
            get => (string)GetValue(IconUrlProperty);

            set => SetValue(IconUrlProperty, value);
        }

        /// <summary>
        /// JS脚本
        /// </summary>
        [AllowNull]
        public string JavaScript
        {
            get => (string)GetValue(JavaScriptProperty);

            set => SetValue(JavaScriptProperty, value);
        }

        /// <summary>
        /// 指示是否在导航栏中显示
        /// </summary>
        public bool ShowInNavView
        {
            get => (bool)GetValue(ShowInNavViewProperty);

            set => SetValue(ShowInNavViewProperty, value);
        }

        /// <summary>
        /// 获取用户编辑后的入口对象
        /// </summary>
        /// <returns>编辑后的入口对象/returns>
        public async Task<WebViewEntry?> GetWebViewEntryAsync()
        {
            if (await ShowAsync() != ContentDialogResult.Secondary)
            {
                if (NavigateUrl is not null)
                {
                    if (JavaScript is not null)
                    {
                        JavaScript = new Regex("(\r\n|\r|\n)").Replace(JavaScript, WhiteSpace);
                        JavaScript = new Regex(@"\s+").Replace(JavaScript, WhiteSpace);
                    }

                    return new WebViewEntry(this);
                }
            }

            return null;
        }

        private void AutoTitleIconButtonClick(object sender, RoutedEventArgs e)
        {
            FindTitleIconAsync().Forget();
        }

        private async Task FindTitleIconAsync()
        {
            if (NavigateUrl is not null)
            {
                using (HttpClient client = new())
                {
                    if (Uri.TryCreate(NavigateUrl, UriKind.Absolute, out Uri? navigateUri))
                    {
                        string response;
                        try
                        {
                            response = await client.GetStringAsync(navigateUri);
                        }
                        catch
                        {
                            response = string.Empty;
                        }

                        Match m;

                        // 匹配标题
                        m = new Regex("(?<=<title>)(.*)(?=</title>)").Match(response);
                        EntryName = m.Success ? m.Value : "自动获取失败";

                        // 匹配图标
                        m = new Regex("(?<=rel[ =]+\"[shortcut icon]+\" href=\")(.*?)(?=\")").Match(response);

                        if (m.Success)
                        {
                            string? path = m.Value;
                            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out Uri? pathUri))
                            {
                                Uri iconUri = pathUri.IsAbsoluteUri
                                    ? pathUri
                                    : new Uri(new Uri(NavigateUrl), pathUri);
                                IconUrl = iconUri.ToString();
                            }
                        }
                    }
                    else
                    {
                        NavigateUrl = "该Url无效";
                    }
                }
            }
        }
    }
}