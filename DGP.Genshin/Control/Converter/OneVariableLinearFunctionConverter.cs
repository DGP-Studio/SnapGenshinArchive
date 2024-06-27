using System.Globalization;

namespace DGP.Genshin.Control.Converter
{
    /// <summary>
    /// 一元线性函数转换器
    /// Y = A * X + B
    /// </summary>
    public sealed class OneVariableLinearFunctionConverter : DependencyObject, IValueConverter
    {
        private static readonly DependencyProperty AProperty = Property<OneVariableLinearFunctionConverter>.Depend(nameof(A), 0D);
        private static readonly DependencyProperty BProperty = Property<OneVariableLinearFunctionConverter>.Depend(nameof(B), 0D);

        /// <summary>
        /// A
        /// </summary>
        public double A
        {
            get => (double)GetValue(AProperty);
            set => SetValue(AProperty, value);
        }

        /// <summary>
        /// B
        /// </summary>
        public double B
        {
            get => (double)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (A * (double)value) + B;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw Must.NeverHappen();
        }
    }
}