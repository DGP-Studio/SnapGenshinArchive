using System.Windows.Media.Animation;

namespace DGP.Genshin.Helper
{
    /// <summary>
    /// 动画帮助类
    /// https://github.com/HandyOrg/HandyControl/blob/master/src/Shared/HandyControl_Shared/Tools/Helper/AnimationHelper.cs
    /// </summary>
    internal class AnimationHelper
    {
        /// <summary>
        /// 创建一个指定的 <see cref="DoubleAnimation"/>
        /// </summary>
        /// <param name="toValue"><see cref="DoubleAnimation.To"/> 的值</param>
        /// <param name="milliseconds">动画时间（毫秒）</param>
        /// <returns>创建的动画</returns>
        public static DoubleAnimation CreateAnimation(double toValue, double milliseconds = 200)
        {
            return new(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut },
            };
        }

        /// <summary>
        /// 创建一个指定的 <see cref="DoubleAnimation"/>
        /// </summary>
        /// <typeparam name="TEasingFunction">缓动动画的类型</typeparam>
        /// <param name="toValue"><see cref="DoubleAnimation.To"/> 的值</param>
        /// <param name="milliseconds">动画时间（毫秒）</param>
        /// <param name="fillBehavior"><see cref=""/>填充行为</param>
        /// <returns>创建的动画</returns>
        public static DoubleAnimation CreateAnimation<TEasingFunction>(double toValue, double milliseconds = 200, FillBehavior fillBehavior = FillBehavior.Stop)
            where TEasingFunction : EasingFunctionBase, new()
        {
            return new(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)), fillBehavior)
            {
                EasingFunction = new TEasingFunction { EasingMode = EasingMode.EaseInOut },
            };
        }
    }
}