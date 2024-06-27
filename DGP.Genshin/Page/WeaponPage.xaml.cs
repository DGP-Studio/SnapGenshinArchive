using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 武器页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class WeaponPage : ModernWpf.Controls.Page
    {
        /// <summary>
        /// 武器页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public WeaponPage(MetadataViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}