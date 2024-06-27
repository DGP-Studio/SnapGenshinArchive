namespace DGP.Genshin.Message
{
    /// <summary>
    /// 自适应背景不透明度变化消息
    /// </summary>
    public class AdaptiveBackgroundOpacityChangedMessage : TypedMessage<double>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="value">值</param>
        public AdaptiveBackgroundOpacityChangedMessage(double value)
            : base(value)
        {
        }
    }
}