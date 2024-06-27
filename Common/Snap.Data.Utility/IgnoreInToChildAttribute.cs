using System;

namespace Snap.Data.Utility
{
    /// <summary>
    /// 声明在调用 <see cref="ToChild{TParent, TChild}(TParent, Action{TChild}?)"/> 方法时跳过此属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IgnoreInToChildAttribute : Attribute
    {
    }
}