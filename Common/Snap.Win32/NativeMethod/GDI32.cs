using System;
using System.Runtime.InteropServices;

namespace Snap.Win32.NativeMethod
{
    public class GDI32
    {
        #region GetDeviceCaps
        public enum DeviceCaps
        {
            HORZRES = 8,
            VERTRES = 10,
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118
        }
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, DeviceCaps nIndex);
        #endregion

        #region StretchBlt
        [Flags]
        public enum CopyPixelOperation
        {
            NoMirrorBitmap = -2147483648,
            Blackness = 66,
            NotSourceErase = 1114278,
            NotSourceCopy = 3342344,
            SourceErase = 4457256,
            DestinationInvert = 5570569,
            PatInvert = 5898313,
            SourceInvert = 6684742,
            SourceAnd = 8913094,
            MergePaint = 12255782,
            MergeCopy = 12583114,
            SourceCopy = 13369376,
            SourcePaint = 15597702,
            PatCopy = 15728673,
            PatPaint = 16452105,
            Whiteness = 16711778,
            CaptureBlt = 1073741824,
        }

        [DllImport("GDI32.DLL", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXDest, int nYDest, int nDestWidth, int nDestHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int nSrcWidth, int nSrcHeight, CopyPixelOperation dwRop);
        #endregion
    }
}
