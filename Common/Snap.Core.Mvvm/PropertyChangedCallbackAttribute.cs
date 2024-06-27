namespace Snap.Core.Mvvm
{
    /// <summary>
    /// 合约特性
    /// 指示此方法为属性改变后执行的回调
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PropertyChangedCallbackAttribute : Attribute
    {
    }
}