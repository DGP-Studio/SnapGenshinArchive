using System.Globalization;

namespace DGP.Genshin.Control.Converter
{
    /// <summary>
    /// 将 <see cref="bool"/> 值取反
    /// </summary>
    public sealed class BooleanRevertConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}