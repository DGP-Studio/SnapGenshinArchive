namespace DGP.Genshin.Control.Helper
{
    /// <summary>
    /// Segou Fluent Icons 图标帮助类
    /// </summary>
    public sealed class IconHelper
    {
        private static readonly DependencyProperty IconProperty = Property<IconHelper>.Attach("Icon", string.Empty);

        /// <summary>
        /// 获取字符图标
        /// </summary>
        /// <param name="obj">待获取的对象</param>
        /// <returns>图标对应的字符</returns>
        public static string GetIcon(DependencyObject obj)
        {
            return (string)obj.GetValue(IconProperty);
        }

        /// <summary>
        /// 设置字符图标
        /// </summary>
        /// <param name="obj">待设置的对象</param>
        /// <param name="value">新的图标字符</param>
        public static void SetIcon(DependencyObject obj, string value)
        {
            obj.SetValue(IconProperty, value);
        }
    }
}