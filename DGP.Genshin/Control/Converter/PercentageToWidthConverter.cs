using System.Globalization;

namespace DGP.Genshin.Control.Converter
{
    /// <summary>
    /// 百分比转宽度
    /// </summary>
    public sealed class PercentageToWidthConverter : DependencyObject, IValueConverter
    {
        private static readonly DependencyProperty TargetWidthProperty = Property<PercentageToWidthConverter>.Depend(nameof(TargetWidth), 1080D);
        private static readonly DependencyProperty TargetHeightProperty = Property<PercentageToWidthConverter>.Depend(nameof(TargetHeight), 390D);

        /// <summary>
        /// 目标宽度
        /// </summary>
        public double TargetWidth
        {
            get => (double)GetValue(TargetWidthProperty);

            set => SetValue(TargetWidthProperty, value);
        }

        /// <summary>
        /// 目标高度
        /// </summary>
        public double TargetHeight
        {
            get => (double)GetValue(TargetHeightProperty);

            set => SetValue(TargetHeightProperty, value);
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * (TargetWidth / TargetHeight);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw Must.NeverHappen();
        }
    }
}