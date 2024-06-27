using System.Collections;
using System.Globalization;

namespace DGP.Genshin.Control.Converter
{
    /// <summary>
    /// 数据可见性转换器，当存在数据时可见
    /// 当源数据为 <see cref="ICollection"/> 时，集合为空也视为不存在数据
    /// </summary>
    public sealed class DataVisibilityRevertConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                ICollection collection => collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                int integer => integer != 0 ? Visibility.Visible : Visibility.Collapsed,
                _ => value != null ? Visibility.Visible : Visibility.Collapsed,
            };
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw Must.NeverHappen();
        }
    }
}