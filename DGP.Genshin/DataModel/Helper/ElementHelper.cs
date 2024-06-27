namespace DGP.Genshin.DataModel.Helper
{
    /// <summary>
    /// 元素帮助类
    /// </summary>
    public class ElementHelper
    {
        /// <summary>
        /// 从英文名称转换为图标链接
        /// </summary>
        /// <param name="element">元素名称</param>
        /// <returns>对应的图标链接</returns>
        public static string FromENGName(string? element)
        {
            return $@"https://genshin.honeyhunterworld.com/img/icons/element/{element?.ToLowerInvariant()}.png";
        }
    }
}