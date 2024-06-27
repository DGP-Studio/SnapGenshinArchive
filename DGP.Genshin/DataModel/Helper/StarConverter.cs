using System.Globalization;

namespace DGP.Genshin.DataModel.Helper
{
    /// <summary>
    /// 将 <see cref="int"/> 类型的等级转化为图标链接
    /// </summary>
    public class StarConverter : IValueConverter
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