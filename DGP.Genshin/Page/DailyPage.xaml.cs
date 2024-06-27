using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 日常页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class DailyPage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的日常页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public DailyPage(DailyViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }
    }
}