namespace DGP.Genshin.Message
{
    /// <summary>
    /// 表示当前的Cookie发生变化
    /// </summary>
    public class CookieChangedMessage : TypedMessage<string>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="cookie">值</param>
        public CookieChangedMessage(string cookie)
            : base(cookie)
        {
        }
    }
}