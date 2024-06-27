namespace DGP.Genshin.Message
{
    /// <summary>
    /// 背景透明度更改事件
    /// </summary>
    public class BackgroundOpacityChangedMessage : TypedMessage<double>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="value">值</param>
        public BackgroundOpacityChangedMessage(double value)
            : base(value)
        {
        }
    }
}