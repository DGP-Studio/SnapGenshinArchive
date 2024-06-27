using System;

namespace Snap.Core.DependencyInjection
{
    /// <summary>
    /// 指示该类为视图模型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelAttribute : InjectableAttribute
    {
        /// <summary>
        /// 指示该类为视图模型
        /// </summary>
        /// <param name="injectAs">指示注入方法</param>
        public ViewModelAttribute(InjectAs injectAs)
        {
            InjectAs = injectAs;
        }
    }
}