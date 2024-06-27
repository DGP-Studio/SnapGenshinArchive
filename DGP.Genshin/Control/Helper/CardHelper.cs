using WPFUI.Controls;

namespace DGP.Genshin.Control.Helper
{
    /// <summary>
    /// <see cref="Card"/> 的帮助类
    /// </summary>
    public sealed class CardHelper
    {
        private static readonly DependencyProperty ActionTitleProperty = Property<CardHelper>.Attach("ActionTitle", string.Empty);
        private static readonly DependencyProperty ActionSubtitleProperty = Property<CardHelper>.Attach("ActionSubtitle", string.Empty);

        /// <summary>
        /// 获取 <see cref="CardAction"/> 的标题
        /// </summary>
        /// <param name="obj">待获取标题的<see cref="CardAction"/></param>
        /// <returns>标题</returns>
        public static string GetActionTitle(CardAction obj)
        {
            return (string)obj.GetValue(ActionTitleProperty);
        }

        /// <summary>
        /// 设置 <see cref="CardAction"/> 的标题
        /// </summary>
        /// <param name="obj">待设置标题的<see cref="CardAction"/></param>
        /// <param name="value">新的标题</param>
        public static void SetActionTitle(CardAction obj, string value)
        {
            obj.SetValue(ActionTitleProperty, value);
        }

        /// <summary>
        /// 获取 <see cref="CardAction"/> 的副标题
        /// </summary>
        /// <param name="obj">待获取副标题的<see cref="CardAction"/></param>
        /// <returns>副标题</returns>
        public static string GetActionSubtitle(CardAction obj)
        {
            return (string)obj.GetValue(ActionSubtitleProperty);
        }

        /// <summary>
        /// 设置 <see cref="CardAction"/> 的副标题
        /// </summary>
        /// <param name="obj">待设置副标题的<see cref="CardAction"/></param>
        /// <param name="value">新的副标题</param>
        public static void SetActionSubtitle(CardAction obj, string value)
        {
            obj.SetValue(ActionSubtitleProperty, value);
        }
    }
}