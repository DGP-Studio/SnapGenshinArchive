namespace Snap.Core.DependencyInjection
{
    /// <summary>
    /// 指示被标注的类型
    /// 生命周期由容器托管
    /// </summary>
    public class LifeCycleAttribute : InjectableAttribute
    {
        /// <summary>
        /// 指示被标注的类型的生命周期由容器托管
        /// </summary>
        /// <param name="injectAs">指示注入方法</param>
        public LifeCycleAttribute(InjectAs injectAs)
        {
            InjectAs = injectAs;
        }
    }
}