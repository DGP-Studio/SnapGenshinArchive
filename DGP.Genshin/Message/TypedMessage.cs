namespace DGP.Genshin.Message
{
    /// <summary>
    /// 为类型消息提供基类
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class TypedMessage<T>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="value">值</param>
        public TypedMessage(T value)
        {
            Value = value;
        }

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; }
    }
}