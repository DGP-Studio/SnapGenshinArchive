using System;

namespace Snap.Threading
{
    /// <summary>
    /// 合约特性 指示该方法需要在主线程上调用
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteOnMainThreadAttribute : Attribute
    {
    }
}