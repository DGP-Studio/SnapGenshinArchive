using System.Globalization;

namespace DGP.Genshin.Control.Converter
{
    /// <summary>
    /// 实现与 <see cref="System.Windows.Controls.BooleanToVisibilityConverter"/> 相反的转换
    /// </summary>
    public sealed class BooleanToVisibilityRevertConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool boolean)
            {
                flag = boolean;
            }
            else if (value is bool?)
            {
                bool? flag2 = (bool?)value;
                flag = flag2.HasValue && flag2.Value;
            }

            return flag ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility != Visibility.Visible;
            }

            return false;
        }
    }
}