using System.Security.Cryptography;
using System.Text;
using System.Windows.Media;

namespace DGP.Genshin.DataModel.GachaStatistic.Item
{
    /// <summary>
    /// 带有个数统计的奖池统计5星物品
    /// </summary>
    public class StatisticItem5Star
    {
        /// <summary>
        /// 图标Url
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 用于记录垫抽数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 是否为大保底
        /// </summary>
        public bool IsBigGuarantee { get; set; }

        /// <summary>
        /// 是否为Up
        /// </summary>
        public bool IsUp { get; set; }

        /// <summary>
        /// 卡池名称
        /// </summary>
        public string? GachaTypeName { get; set; }

        /// <summary>
        /// 背景
        /// </summary>
        public SolidColorBrush Background
        {
            get
            {
                byte[] codes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Name!));
                Color color = Color.FromRgb(
                    (byte)((codes[0] / 2) + 64),
                    (byte)((codes[1] / 2) + 64),
                    (byte)((codes[2] / 2) + 64));
                return new SolidColorBrush(color);
            }
        }

        /// <summary>
        /// 半透明背景
        /// </summary>
        public SolidColorBrush TranslucentBackground
        {
            get
            {
                byte[] codes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Name!));
                Color color = Color.FromArgb(
                    102,
                    (byte)((codes[0] / 2) + 64),
                    (byte)((codes[1] / 2) + 64),
                    (byte)((codes[2] / 2) + 64));
                return new SolidColorBrush(color);
            }
        }
    }
}