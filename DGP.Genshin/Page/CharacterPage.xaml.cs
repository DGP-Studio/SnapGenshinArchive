using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 角色页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class CharacterPage : ModernWpf.Controls.Page
    {
        /// <summary>
        /// 构造一个新的角色页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public CharacterPage(MetadataViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}