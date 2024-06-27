using DGP.Genshin.DataModel.Helper;
using System.Globalization;

namespace DGP.Genshin.Control.Converter
{
    /// <summary>
    /// 将 稀有度数字转换到 底图Url
    /// </summary>
    public sealed class RarityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StarHelper.FromInt32Rank((int)value);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw Must.NeverHappen();
        }
    }
}