namespace DGP.Genshin.Message
{
    /// <summary>
    /// 新增Cookie
    /// </summary>
    public class CookieAddedMessage : TypedMessage<string>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="cookie">值</param>
        public CookieAddedMessage(string cookie)
            : base(cookie)
        {
        }
    }
}