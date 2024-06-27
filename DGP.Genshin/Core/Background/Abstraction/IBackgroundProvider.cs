using DGP.Genshin.Core.ImplementationSwitching;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DGP.Genshin.Core.Background.Abstraction
{
    /// <summary>
    /// 背景提供器接口
    /// 实现此接口的对象必须包含公共无参构造器
    /// </summary>
    [SwitchableInterfaceDefinitionContract]
    public interface IBackgroundProvider
    {
        /// <summary>
        /// 获取下一张壁纸
        /// </summary>
        /// <returns>下一张壁纸 或 <see langword="null"/> 如果获取失败</returns>
        Task<BitmapImage?> GetNextBitmapImageAsync();
    }
}