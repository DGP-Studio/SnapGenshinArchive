using System;
using System.Runtime.InteropServices;

using static Snap.Win32.NativeMethod.User32;

namespace Snap.Win32
{
    public static class Clipboard2
    {
        /// <summary>
        /// 更好的复制到剪贴板
        /// </summary>
        /// <param name="text">待复制的文本</param>
        /// <returns>是否成功</returns>
        public static bool SetText(string text)
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                return false;
            }

            IntPtr global = Marshal.StringToHGlobalUni(text);

            SetClipboardData(CF_UNICODETEXT, global);
            CloseClipboard();
            Marshal.FreeHGlobal(global);
            return true;
        }
    }
}
