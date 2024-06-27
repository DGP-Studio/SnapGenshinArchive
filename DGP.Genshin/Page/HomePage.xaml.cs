using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 主页
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class HomePage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的主页
        /// </summary>
        /// <param name="vm">视图模型</param>
        public HomePage(HomeViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }
    }
}