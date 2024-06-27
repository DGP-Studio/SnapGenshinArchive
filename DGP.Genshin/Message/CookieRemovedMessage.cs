namespace DGP.Genshin.Message
{
    /// <summary>
    /// 删除Cookie
    /// </summary>
    public class CookieRemovedMessage : TypedMessage<string>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="cookie">值</param>
        public CookieRemovedMessage(string cookie)
            : base(cookie)
        {
        }
    }
}