using DGP.Genshin.DataModel.Promotion;
using ModernWpf.Controls;

namespace DGP.Genshin.Control.GenshinElement.PromotionCalculate
{
    /// <summary>
    /// 材料清单编辑对话框
    /// </summary>
    public partial class MaterialListDialog : ContentDialog
    {
        /// <summary>
        /// 构造一个新的材料清单编辑对话框
        /// </summary>
        /// <param name="calculableConsume">编辑的目标清单</param>
        public MaterialListDialog(CalculableConsume calculableConsume)
        {
            DataContext = calculableConsume;
            InitializeComponent();
        }
    }
}