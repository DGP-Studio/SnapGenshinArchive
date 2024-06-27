using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 插件管理页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class PluginPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// 构造一个新的插件管理页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public PluginPage(PluginViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}