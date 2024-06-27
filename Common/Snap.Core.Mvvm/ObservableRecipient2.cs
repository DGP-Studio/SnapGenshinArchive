using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Snap.Core.Mvvm
{
    /// <summary>
    /// 自动设置 <see cref="ObservableRecipient.IsActive"/>
    /// </summary>
    public class ObservableRecipient2 : ObservableRecipient
    {
        /// <inheritdoc cref="ObservableRecipient"/>
        public ObservableRecipient2(IMessenger messenger)
            : base(messenger)
        {
            IsActive = true;
        }

        /// <inheritdoc cref="ObservableRecipient"/>
        ~ObservableRecipient2()
        {
            IsActive = false;
        }

        /// <summary>
        /// <inheritdoc cref="ObservableObject.SetProperty{T}(ref T, T, string?)"/>
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="field">属性对应的字段</param>
        /// <param name="newValue">新的值</param>
        /// <param name="then">操作成功后执行的回调</param>
        /// <param name="propertyName">属性名称，留空以使编译服务自动填充</param>
        /// <returns>值是否发生了变化</returns>
        protected bool SetPropertyAndCallbackOnCompletion<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, Action then, [CallerMemberName] string? propertyName = null)
        {
            bool result = SetProperty(ref field, newValue, propertyName);
            if (result)
            {
                then.Invoke();
            }

            return result;
        }

        /// <summary>
        /// <inheritdoc cref="ObservableObject.SetProperty{T}(ref T, T, string?)"/>
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="field">属性对应的字段</param>
        /// <param name="newValue">新的值</param>
        /// <param name="then">操作成功后执行的回调</param>
        /// <param name="propertyName">属性名称，留空以使编译服务自动填充</param>
        /// <returns>值是否发生了变化</returns>
        protected bool SetPropertyAndCallbackOnCompletion<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, Action<T> then, [CallerMemberName] string? propertyName = null)
        {
            bool result = SetProperty(ref field, newValue, propertyName);
            if (result)
            {
                then.Invoke(newValue);
            }

            return result;
        }

        /// <summary>
        /// <inheritdoc cref="ObservableObject.SetProperty{T}(ref T, T, string?)"/>
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="field">属性对应的字段</param>
        /// <param name="newValue">新的值</param>
        /// <param name="thenForget">操作成功后执行的回调</param>
        /// <param name="propertyName">属性名称，留空以使编译服务自动填充</param>
        /// <returns>值是否发生了变化</returns>
        protected bool SetPropertyAndCallbackOnCompletion<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, Func<T, Task> thenForget, [CallerMemberName] string? propertyName = null)
        {
            return SetPropertyAndCallbackOnCompletion(ref field, newValue, thenForget.Invoke(newValue).Forget, propertyName);
        }

        /// <summary>
        /// <inheritdoc cref="ObservableObject.SetProperty{T}(ref T, T, string?)"/>
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="field">属性对应的字段</param>
        /// <param name="newValue">新的值</param>
        /// <param name="then">操作成功后执行的回调</param>
        /// <param name="propertyName">属性名称，留空以使编译服务自动填充</param>
        /// <returns>值是否发生了变化</returns>
        protected bool SetPropertyAndCallbackOverridePropertyState<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, Action then, [CallerMemberName] string? propertyName = null)
        {
            bool result = SetProperty(ref field, newValue, propertyName);
            then.Invoke();
            return result;
        }
    }
}