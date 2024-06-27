using ModernWpf.Controls;
using Snap.Data.Primitive;
using System.Threading.Tasks;

namespace DGP.Genshin.Control.GenshinElement.GachaStatistic
{
    /// <summary>
    /// 祈愿记录手动Url对话框
    /// </summary>
    public sealed partial class GachaLogUrlDialog : ContentDialog
    {
        /// <summary>
        /// 构造一个新的祈愿记录手动Url对话框
        /// </summary>
        public GachaLogUrlDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 异步获取用户输入的Url
        /// </summary>
        /// <returns>用户输入的结果</returns>
        public async Task<Result<bool, string>> GetInputUrlAsync()
        {
            bool isOk = await ShowAsync() != ContentDialogResult.Secondary;
            return Results.Condition(isOk, InputText.Text, string.Empty);
        }
    }
}