using System;

namespace Snap.Core.DependencyInjection
{
    /// <summary>
    /// 指示被标注的类型可注入，且带有接口实现
    /// </summary>
    public abstract class InterfaceInjectableAttribute : InjectableAttribute
    {
        /// <summary>
        /// 该类实现的接口类型
        /// </summary>
        public Type InterfaceType { get; set; } = default!;
    }
}