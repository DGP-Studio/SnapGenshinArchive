using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 自定义网页管理页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class WebViewLobbyPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// 构造一个新的自定义网页管理页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public WebViewLobbyPage(WebViewLobbyViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}