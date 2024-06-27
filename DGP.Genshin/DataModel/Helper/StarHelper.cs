using System.Windows.Media;

namespace DGP.Genshin.DataModel.Helper
{
    /// <summary>
    /// 等级帮助类
    /// </summary>
    public static class StarHelper
    {
        private const string Star1Url = @"https://genshin.honeyhunterworld.com/img/back/item/1star.png";
        private const string Star2Url = @"https://genshin.honeyhunterworld.com/img/back/item/2star.png";
        private const string Star3Url = @"https://genshin.honeyhunterworld.com/img/back/item/3star.png";
        private const string Star4Url = @"https://genshin.honeyhunterworld.com/img/back/item/4star.png";
        private const string Star5Url = @"https://genshin.honeyhunterworld.com/img/back/item/5star.png";

        /// <summary>
        /// 将 <see cref="int"/> 类型的等级转化为图标链接
        /// </summary>
        /// <param name="rank">等级</param>
        /// <returns>图标链接</returns>
        public static string FromInt32Rank(int rank)
        {
            return rank switch
            {
                1 => Star1Url,
                2 => Star2Url,
                3 => Star3Url,
                4 => Star4Url,
                5 or 105 => Star5Url,
                _ => throw Must.NeverHappen(),
            };
        }

        /// <summary>
        /// 将 图标链接 转化为 <see cref="int"/> 类型的等级
        /// </summary>
        /// <param name="starurl">图标链接</param>
        /// <returns>等级</returns>
        public static int ToInt32Rank(this string? starurl)
        {
            return starurl is null ? 1 : int.Parse(starurl[51].ToString());
        }

        /// <summary>
        /// 判断字符串是否为对应的等级
        /// </summary>
        /// <param name="starurl">待判断的字符串</param>
        /// <param name="rank">待判断的等级</param>
        /// <returns>是否相等</returns>
        public static bool IsRankAs(this string? starurl, int rank)
        {
            return starurl.ToInt32Rank() == rank;
        }

        /// <summary>
        /// 将 <see cref="int"/> 类型的等级转化为对应颜色的画刷
        /// </summary>
        /// <param name="rank">等级</param>
        /// <returns>对应颜色的画刷</returns>
        public static SolidColorBrush? ToSolid(int rank)
        {
            Color color = rank switch
            {
                1 => Color.FromRgb(114, 119, 139),
                2 => Color.FromRgb(42, 143, 114),
                3 => Color.FromRgb(81, 128, 203),
                4 => Color.FromRgb(161, 86, 224),
                5 or 105 => Color.FromRgb(188, 105, 50),
                _ => Color.FromRgb(114, 119, 139),
            };
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// 将 图标Url 转化为对应颜色的画刷
        /// </summary>
        /// <param name="starurl">等级</param>
        /// <returns>对应颜色的画刷</returns>
        public static SolidColorBrush? ToSolid(string? starurl)
        {
            return starurl is null ? null : ToSolid(ToInt32Rank(starurl));
        }

        /// <summary>
        /// 将 <see cref="int"/> 类型的等级转化为对应图标的 <see cref="Uri"/>
        /// </summary>
        /// <param name="rank">等级</param>
        /// <returns>对应图标的 <see cref="Uri"/></returns>
        public static Uri? ToUri(int rank)
        {
            string? rankUrl = FromInt32Rank(rank);
            return rankUrl is null ? null : new Uri(rankUrl);
        }
    }
}