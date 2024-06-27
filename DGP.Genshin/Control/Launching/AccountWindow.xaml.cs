using DGP.Genshin.ViewModel;

namespace DGP.Genshin.Control.Launching
{
    /// <summary>
    /// 选择账户窗口
    /// </summary>
    internal partial class AccountWindow : Window
    {
        internal AccountWindow(LaunchViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        private void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}