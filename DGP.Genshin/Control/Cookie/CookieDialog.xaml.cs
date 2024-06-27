using ModernWpf.Controls;
using Snap.Data.Primitive;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DGP.Genshin.Control.Cookie
{
    /// <summary>
    /// Cookie对话框
    /// </summary>
    public sealed partial class CookieDialog : ContentDialog
    {
        private static readonly DependencyProperty IsAutoWindowOptionVisibleProperty = Property<CookieDialog>.Depend(nameof(IsAutoWindowOptionVisible), true);

        /// <summary>
        /// 构造一个新的对话框实例
        /// </summary>
        /// <param name="isAutoWindowOptionVisible">自动获取Cookie模式入口是否可见</param>
        public CookieDialog(bool isAutoWindowOptionVisible = true)
        {
            IsAutoWindowOptionVisible = isAutoWindowOptionVisible;
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// 自动获取Cookie模式入口是否可见
        /// </summary>
        public bool IsAutoWindowOptionVisible
        {
            get => (bool)GetValue(IsAutoWindowOptionVisibleProperty);
            set => SetValue(IsAutoWindowOptionVisibleProperty, value);
        }

        /// <summary>
        /// 获取输入的Cookie
        /// </summary>
        /// <returns>输入的结果</returns>
        public async Task<Result<bool, string>> GetInputCookieAsync()
        {
            ContentDialogResult result = await ShowAsync();
            string cookie = InputText.Text;

            return new(result != ContentDialogResult.Secondary, cookie);
        }

        private void AutoCookieButtonClick(object sender, RoutedEventArgs e)
        {
            if (!WebView2Helper.IsSupported)
            {
                new WebView2RuntimeWindow().ShowDialog();
            }
            else
            {
                using (CookieWindow cookieWindow = new())
                {
                    cookieWindow.ShowDialog();
                    if (cookieWindow.IsLoggedIn)
                    {
                        InputText.Text = cookieWindow.Cookie;
                    }
                }
            }
        }

        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = InputText.Text;

            bool inputEmpty = string.IsNullOrEmpty(text);
            bool inputHasAccountId = text.Contains("account_id");

            (PrimaryButtonText, IsPrimaryButtonEnabled) = (inputEmpty, inputHasAccountId) switch
            {
                (true, _) => ("请输入Cookie", false),
                (false, true) => ("确认", true),
                (false, false) => ("该Cookie无效", false),
            };
        }
    }
}