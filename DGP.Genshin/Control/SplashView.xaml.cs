using DGP.Genshin.ViewModel;
using System.Windows.Controls;

namespace DGP.Genshin.Control
{
    /// <summary>
    /// 初始化视图
    /// </summary>
    public sealed partial class SplashView : UserControl
    {
        /// <summary>
        /// 构造一个新的初始化视图
        /// </summary>
        public SplashView()
        {
            DataContext = App.AutoWired<SplashViewModel>();
            InitializeComponent();
        }
    }
}