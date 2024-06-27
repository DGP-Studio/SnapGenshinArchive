using Snap.Win32.NativeMethod;
using System.Windows.Interop;

namespace DGP.Genshin.Core.DpiAware
{
    /// <summary>
    /// 分辨率感知
    /// </summary>
    internal static class DpiAware
    {
        private static bool? isDpiMethodSupported = null;

        /// <summary>
        /// 是否支持分辨率感知
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                isDpiMethodSupported ??= DoesWin32MethodExist("shcore.dll", "SetProcessDpiAwareness");
                return isDpiMethodSupported.Value;
            }
        }

        /// <summary>
        /// 获取缩放比
        /// </summary>
        /// <param name="window">目标窗体</param>
        /// <returns>缩放比</returns>
        public static double GetScaleRatio(Window window)
        {
            PresentationSource hwndSource = PresentationSource.FromVisual(window);

            // TODO: verify use hwndSource there
            double wpfDpi = 96.0 * PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice.M11;

            if (IsSupported == false)
            {
                // Use System DPI
                return wpfDpi / 96.0;
            }
            else
            {
                IntPtr monitor = User32.MonitorFromWindow(((HwndSource)hwndSource).Handle, User32.MonitorOpts.MONITOR_DEFAULTTONEAREST);
                _ = SHCore.GetDpiForMonitor(monitor, SHCore.MonitorDpiType.MDT_EFFECTIVE_DPI, out uint dpiX, out uint _);
                return dpiX / wpfDpi;
            }
        }

        private static bool DoesWin32MethodExist(string moduleName, string methodName)
        {
            IntPtr hModule = Kernel32.LoadLibrary(moduleName);

            if (hModule != IntPtr.Zero)
            {
                IntPtr functionPointer = Kernel32.GetProcAddress(hModule, methodName);
                Kernel32.FreeLibrary(hModule);
                return (functionPointer != IntPtr.Zero);
            }
            else
            {
                return false;
            }
        }
    }
}