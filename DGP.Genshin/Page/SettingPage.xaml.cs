using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 设置页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class SettingPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// 构造一个新的设置页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public SettingPage(SettingViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}