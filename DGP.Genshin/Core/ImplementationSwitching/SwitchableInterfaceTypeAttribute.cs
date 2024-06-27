namespace DGP.Genshin.Core.ImplementationSwitching
{
    /// <summary>
    /// 标记可切换集合，告知类型检测服务可切换服务的类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class SwitchableInterfaceTypeAttribute : Attribute
    {
        /// <summary>
        /// 标记可切换集合，告知类型检测服务可切换服务的类型
        /// </summary>
        /// <param name="type">可切换服务的类型</param>
        public SwitchableInterfaceTypeAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// 可切换服务的类型
        /// </summary>
        public Type Type { get; }
    }
}