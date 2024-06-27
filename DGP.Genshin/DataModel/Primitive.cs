using DGP.Genshin.DataModel.Helper;
using System.Windows.Media;

namespace DGP.Genshin.DataModel
{
    /// <summary>
    /// 物品元件，在 <see cref="KeySource"/> 的基础上增加了名称与星级
    /// </summary>
    public abstract class Primitive : KeySource
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 等级底图Url
        /// </summary>
        public string? Star { get; set; } = StarHelper.FromInt32Rank(1);

        /// <summary>
        /// 等级底色
        /// </summary>
        public SolidColorBrush? StarSolid
        {
            get => StarHelper.ToSolid(Star);
        }

        /// <summary>
        /// 获取角标Url
        /// </summary>
        /// <returns>角标Url</returns>
        public abstract string? GetBadge();
    }
}