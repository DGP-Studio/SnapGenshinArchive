namespace DGP.Genshin.Core.Plugins
{
    /// <summary>
    /// 实现插件接口
    /// 该类必须具有一个公共的无参构造函数
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 启用与禁用
        /// 插件需要自行实现这一状态的保存
        /// </summary>
        [Obsolete("该属性不再有意义")]
        bool IsEnabled { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 作者
        /// </summary>
        string Author { get; }

        /// <summary>
        /// 版本
        /// </summary>
        Version Version { get; }
    }
}