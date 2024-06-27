using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 账号管理页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class UserPage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的账号管理页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public UserPage(UserViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }
    }
}