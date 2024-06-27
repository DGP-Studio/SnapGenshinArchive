using System.Windows.Interop;
using static Snap.Win32.NativeMethod.User32;

namespace DGP.Genshin.Helper.Extension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class WindowSinkExtensions
    {
        /// <summary>
        /// 使窗体置于桌面底端
        /// </summary>
        /// <param name="window">目标窗口</param>
        public static void SetInDesktop(this Window window)
        {
            IntPtr hWnd = new WindowInteropHelper(window).Handle;

            // notify windows to create a WorkerW
            IntPtr hProgManWnd = FindWindow("Progman", "Program Manager");
            SendMessageTimeout(hProgManWnd, 1324u, new UIntPtr(0u), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 1000u, out UIntPtr _);
            ConfigureWorkerW();
            SetParent(hWnd, hProgManWnd);
        }

        private static void ConfigureWorkerW()
        {
            IntPtr hWorkerWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "WorkerW", IntPtr.Zero);

            while (hWorkerWnd != IntPtr.Zero)
            {
                uint dwStyle = GetWindowLong(hWorkerWnd, GWL_STYLE);
                if ((dwStyle & WS_VISIBLE) != 0)
                {
                    IntPtr hdefWnd = FindWindowEx(hWorkerWnd, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero);
                    if (hdefWnd != IntPtr.Zero)
                    {
                        hWorkerWnd = FindWindowEx(IntPtr.Zero, hWorkerWnd, "WorkerW", IntPtr.Zero);
                        break;
                    }
                }

                hWorkerWnd = FindWindowEx(IntPtr.Zero, hWorkerWnd, "WorkerW", IntPtr.Zero);
            }

            _ = ShowWindow(hWorkerWnd, SW_HIDE);
        }
    }
}