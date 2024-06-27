using System;
using System.Runtime.InteropServices;

namespace Snap.Win32.NativeMethod
{
    public class SHCore
    {
        #region SetProcessDpiAwareness
        public enum PROCESS_DPI_AWARENESS
        {
            PROCESS_DPI_UNAWARE = 0,
            PROCESS_SYSTEM_DPI_AWARE = 1,
            PROCESS_PER_MONITOR_DPI_AWARE = 2
        }

        [DllImport("shcore.dll")]
        public static extern uint SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);
        #endregion

        #region
        public enum MonitorDpiType
        {
            MDT_EFFECTIVE_DPI = 0,
            MDT_ANGULAR_DPI = 1,
            MDT_RAW_DPI = 2,
        }

        [DllImport("shcore.dll")]
        public static extern uint GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);
        #endregion
    }

}
