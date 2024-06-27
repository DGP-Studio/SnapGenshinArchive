namespace DGP.Genshin.Message
{
    /// <summary>
    /// 请求导航消息
    /// </summary>
    public class NavigateRequestMessage : TypedMessage<Type>
    {
        /// <summary>
        /// 构造一个新的消息
        /// </summary>
        /// <param name="pageType">待导航的页面类型</param>
        /// <param name="isSyncTabRequested">是否要求同步导航项选中，仅当页面在左侧导航栏中存在入口点时才这么做</param>
        /// <param name="extraData">向导航页面发送的额外数据</param>
        public NavigateRequestMessage(Type pageType, bool isSyncTabRequested = false, object? extraData = null)
            : base(pageType)
        {
            IsSyncTabRequested = isSyncTabRequested;
            ExtraData = extraData;
        }

        /// <summary>
        /// 是否要求同步导航项选中
        /// 仅当页面在左侧导航栏中存在入口点时才这么做
        /// </summary>
        internal bool IsSyncTabRequested { get; set; }

        /// <summary>
        /// 向导航页面发送的额外数据
        /// </summary>
        internal object? ExtraData { get; set; }
    }
}