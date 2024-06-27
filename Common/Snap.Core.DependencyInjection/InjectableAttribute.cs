using System;

namespace Snap.Core.DependencyInjection
{
    /// <summary>
    /// 指示被标注的类型可注入
    /// </summary>
    public abstract class InjectableAttribute : Attribute
    {
        /// <summary>
        /// 注入类型
        /// </summary>
        public InjectAs InjectAs { get; set; }
    }
}