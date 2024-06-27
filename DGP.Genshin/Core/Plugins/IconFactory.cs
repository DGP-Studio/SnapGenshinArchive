using ModernWpf.Controls;

namespace DGP.Genshin.Core.Plugins
{
    /// <summary>
    /// 图标工厂
    /// </summary>
    public abstract class IconFactory
    {
        /// <summary>
        /// 获取将要显示的图标
        /// </summary>
        /// <returns>待显示的图标</returns>
        public abstract IconElement? GetIcon();
    }
}