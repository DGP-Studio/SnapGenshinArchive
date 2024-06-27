using System.Runtime.InteropServices;

namespace DGP.Genshin.Helper.Extension
{
    /// <summary>
    /// 像素
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Pixel
    {
        /// <summary>
        /// 蓝色通道
        /// </summary>
        public byte Blue;

        /// <summary>
        /// 绿色通道
        /// </summary>
        public byte Green;

        /// <summary>
        /// 红色通道
        /// </summary>
        public byte Red;

        /// <summary>
        /// 透明度通道
        /// </summary>
        public byte Alpha;
    }
}