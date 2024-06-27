namespace DGP.Genshin.Core.ImplementationSwitching
{
    /// <summary>
    /// 表示该类作为可切换的实现注入切换上下文
    /// 类型不会被注入到依赖容器中，而是根据默认的公共无参构造器生成实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SwitchableImplementationAttribute : Attribute
    {
        /// <summary>
        /// 默认名称
        /// </summary>
        internal const string DefaultName = "Snap.Genshin.Default";

        /// <summary>
        /// 默认描述
        /// </summary>
        internal const string DefaultDescription = "Snap Genshin 默认实现";

        /// <summary>
        /// 表示标记类作为可切换的实现注入切换上下文
        /// </summary>
        /// <param name="targetInterface">目标实现接口</param>
        /// <param name="name">唯一名称</param>
        /// <param name="description">显示的描述</param>
        public SwitchableImplementationAttribute(Type targetInterface, string name, string description)
        {
            TargetType = targetInterface;

            Requires.Argument(name != DefaultName, nameof(name), "注册的名称不能与默认实现名称相同");
            Name = name;

            Requires.Argument(description != DefaultDescription, nameof(description), "注册的描述不能与默认实现描述相同");
            Description = description;
        }

        /// <summary>
        /// 表示标记类作为可切换的实现注入切换上下文
        /// </summary>
        /// <param name="targetInterface">目标实现接口</param>
        internal SwitchableImplementationAttribute(Type targetInterface)
        {
            TargetType = targetInterface;
            Name = DefaultName;
            Description = DefaultDescription;
        }

        /// <summary>
        /// 目标类型
        /// </summary>
        internal Type TargetType { get; }

        /// <summary>
        /// 唯一名称
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// 展示的描述
        /// </summary>
        internal string Description { get; }
    }
}