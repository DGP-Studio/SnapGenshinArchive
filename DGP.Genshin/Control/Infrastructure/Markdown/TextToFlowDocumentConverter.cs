using System.Globalization;

namespace DGP.Genshin.Control.Infrastructure.Markdown
{
    /// <summary>
    /// 将文本转化为流文档
    /// </summary>
    public class TextToFlowDocumentConverter : DependencyObject, IValueConverter
    {
        private static readonly DependencyProperty MarkdownProperty = Property<TextToFlowDocumentConverter>.Depend<Markdown>(nameof(Markdown));

        private readonly Lazy<Markdown> mMarkdown = new(() => new Markdown());

        /// <summary>
        /// 使用的转换对象
        /// </summary>
        public Markdown? Markdown
        {
            get => (Markdown)GetValue(MarkdownProperty);

            set => SetValue(MarkdownProperty, value);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            if (value is null)
            {
                return null;
            }

            string text = (string)value;

            Markdown engine = Markdown ?? mMarkdown.Value;

            return engine.Transform(text);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw Must.NeverHappen();
        }
    }
}