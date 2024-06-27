using Snap.Win32.NativeMethod;
using System;
using System.Drawing;

namespace Snap.Win32
{
    public class PrimaryScreen
    {
        /// <summary>
        /// 获取真实设置的桌面分辨率大小
        /// </summary>
        public static Size DESKTOP
        {
            get
            {
                IntPtr hdc = User32.GetDC(IntPtr.Zero);
                Size size = new();
                size.Width = GDI32.GetDeviceCaps(hdc, GDI32.DeviceCaps.DESKTOPHORZRES);
                size.Height = GDI32.GetDeviceCaps(hdc, GDI32.DeviceCaps.DESKTOPVERTRES);
                User32.ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>
        /// 获取宽度缩放百分比
        /// </summary>
        public static float ScaleX
        {
            get
            {
                IntPtr hdc = User32.GetDC(IntPtr.Zero);
                float ScaleX = GDI32.GetDeviceCaps(hdc, GDI32.DeviceCaps.DESKTOPHORZRES) / (float)GDI32.GetDeviceCaps(hdc, GDI32.DeviceCaps.HORZRES);
                User32.ReleaseDC(IntPtr.Zero, hdc);
                return ScaleX;
            }
        }
        /// <summary>
        /// 获取高度缩放百分比
        /// </summary>
        public static float ScaleY
        {
            get
            {
                IntPtr hdc = User32.GetDC(IntPtr.Zero);
                float ScaleY = GDI32.GetDeviceCaps(hdc, GDI32.DeviceCaps.DESKTOPVERTRES) / (float)GDI32.GetDeviceCaps(hdc, GDI32.DeviceCaps.VERTRES);
                User32.ReleaseDC(IntPtr.Zero, hdc);
                return ScaleY;
            }
        }
    }
}
