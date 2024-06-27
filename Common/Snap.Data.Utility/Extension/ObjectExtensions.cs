using Snap.Reflection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Snap.Data.Utility.Extension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 将父类对象的属性复制到新创建的子类实例
        /// 使用此方法时 <typeparamref name="TParent"/> 与 <typeparamref name="TChild"/> 中不能存在类索引器 this[]
        /// <para/>
        /// 会忽略带有 <see cref="IgnoreInToChildAttribute"/> 特性的属性
        /// </summary>
        /// <typeparam name="TParent">父类型</typeparam>
        /// <typeparam name="TChild">子类型</typeparam>
        /// <param name="parent">父对象</param>
        /// <param name="additionalModifier">拷贝完成后执行的额外操作</param>
        /// <returns>新的子对象实例</returns>
        [return: NotNullIfNotNull("parent")]
        public static TChild? ToChild<TParent, TChild>(this TParent parent, Action<TChild>? additionalModifier = null)
            where TChild : TParent, new()
        {
            if (parent != null)
            {
                TChild child = new();

                parent.ForEachPropertyInfoWithoutAttribute<IgnoreInToChildAttribute>(parentProp =>
                child.SetPropertyValueByName(parentProp.Name, parent.GetPropertyValueByInfo(parentProp)));

                additionalModifier?.Invoke(child);
                return child;
            }

            return default;
        }
    }
}