using Snap.Core.DependencyInjection;
using SystemPage = System.Windows.Controls.Page;

namespace DGP.Genshin.Sample.Plugin
{
    /// <summary>
    /// 示例页面
    /// </summary>
    [View(InjectAs.Transient)]
    public partial class SamplePage : SystemPage
    {
        /// <summary>
        /// 构造一个新的示例页面
        /// </summary>
        /// <param name="vm">示例视图模型</param>
        public SamplePage(SampleViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}