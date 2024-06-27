using DGP.Genshin.Control.Infrastructure.Observable;
using DGP.Genshin.DataModel.Launching;
using System.Threading.Tasks;

namespace DGP.Genshin.Control.Launching
{
    /// <summary>
    /// 启动游戏账号命名对话框
    /// </summary>
    public sealed partial class NameDialog : ObservableContentDialog
    {
        private string? input;
        private GenshinAccount? targetAccount;

        /// <summary>
        /// 构造一个新的命名对话框
        /// </summary>
        public NameDialog()
        {
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// 输入
        /// </summary>
        public string? Input
        {
            get => input;

            set => Set(ref input, value);
        }

        /// <summary>
        /// 目标账号
        /// </summary>
        public GenshinAccount? TargetAccount
        {
            get => targetAccount;

            set => Set(ref targetAccount, value);
        }

        /// <summary>
        /// 获取用户输入
        /// </summary>
        /// <returns>用户输入的字符串或 <see langword="null"/></returns>
        public async Task<string?> GetInputAsync()
        {
            await ShowAsync();
            return Input;
        }
    }
}