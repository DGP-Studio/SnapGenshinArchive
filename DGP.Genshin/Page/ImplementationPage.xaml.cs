using Snap.Core.DependencyInjection;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 实现管理页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class ImplementationPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// 构造一个新的实现管理页面
        /// </summary>
        public ImplementationPage()
        {
            DataContext = App.Current.SwitchableImplementationManager;
            InitializeComponent();
        }
    }
}