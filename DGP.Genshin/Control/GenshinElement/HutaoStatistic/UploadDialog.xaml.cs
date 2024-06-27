using DGP.Genshin.HutaoAPI.PostModel;
using ModernWpf.Controls;

namespace DGP.Genshin.Control.GenshinElement.HutaoStatistic
{
    /// <summary>
    /// 胡桃数据库上传确认对话框
    /// </summary>
    public sealed partial class UploadDialog : ContentDialog
    {
        /// <summary>
        /// 构造一个新的上传确认对话框
        /// </summary>
        /// <param name="playerRecord">待确认上传的记录</param>
        public UploadDialog(PlayerRecord playerRecord)
        {
            DataContext = playerRecord;
            InitializeComponent();
        }
    }
}