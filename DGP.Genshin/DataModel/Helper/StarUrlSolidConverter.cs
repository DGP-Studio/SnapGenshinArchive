using System.Globalization;

namespace DGP.Genshin.DataModel.Helper
{
    /// <summary>
    /// 将 图标Url转化为图标链接
    /// </summary>
    public class StarUrlSolidConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StarHelper.ToSolid((string)value);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}