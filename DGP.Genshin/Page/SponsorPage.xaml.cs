using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 赞助名单页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class SponsorPage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的赞助名单页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public SponsorPage(SponsorViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }
    }
}