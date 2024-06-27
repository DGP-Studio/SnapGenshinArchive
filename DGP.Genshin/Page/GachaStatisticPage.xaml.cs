using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 祈愿记录界面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class GachaStatisticPage : ModernWpf.Controls.Page
    {
        /// <summary>
        /// 祈愿记录页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public GachaStatisticPage(GachaStatisticViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}