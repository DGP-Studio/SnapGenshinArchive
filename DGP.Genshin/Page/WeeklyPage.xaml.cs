using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 周本页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class WeeklyPage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的周本页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public WeeklyPage(WeeklyViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }
    }
}