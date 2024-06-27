using System.Globalization;

namespace DGP.Genshin.DataModel.Helper
{
    /// <summary>
    /// 将 <see cref="int"/> 类型的等级转化为对应颜色的画刷
    /// </summary>
    public class StarSolidConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StarHelper.ToSolid((int)value);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}