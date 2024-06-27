using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 启动游戏页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class LaunchPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// 构造一个新的启动游戏页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public LaunchPage(LaunchViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}