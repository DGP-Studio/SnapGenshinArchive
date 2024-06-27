using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 胡桃数据库页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class HutaoStatisticPage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的胡桃数据库页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public HutaoStatisticPage(HutaoStatisticViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }
    }
}