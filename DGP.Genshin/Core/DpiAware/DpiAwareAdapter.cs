using Snap.Core.Logging;
using Snap.Win32;
using Snap.Win32.NativeMethod;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;

namespace DGP.Genshin.Core.DpiAware
{
    /// <summary>
    /// 高分辨率适配器
    /// </summary>
    internal class DpiAwareAdapter
    {
        private readonly Window window;

        private HwndSource? hwndSource;
        [SuppressMessage("", "IDE0052")]
        private IntPtr? hwnd;
        private double currentDpiRatio;

        static DpiAwareAdapter()
        {
            if (DpiAware.IsSupported)
            {
                // We need to call this early before we start doing any fiddling with window coordinates / geometry
                _ = SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE);
            }
        }

        /// <summary>
        /// 构造一个新的高分辨率适配器
        /// </summary>
        /// <param name="window">目标窗体</param>
        public DpiAwareAdapter(Window window)
        {
            this.window = window;
            window.Loaded += (_, _) => OnAttached();
            window.Closing += (_, _) => OnDetaching();
        }

        private void OnAttached()
        {
            if (window.IsInitialized)
            {
                AddHwndHook();
            }
            else
            {
                window.SourceInitialized += AssociatedWindowSourceInitialized;
            }
        }

        private void OnDetaching()
        {
            RemoveHwndHook();
        }

        private void AddHwndHook()
        {
            hwndSource = PresentationSource.FromVisual(window) as HwndSource;
            hwndSource?.AddHook(HwndHook);
            hwnd = new WindowInteropHelper(window).Handle;
        }

        private void RemoveHwndHook()
        {
            window.SourceInitialized -= AssociatedWindowSourceInitialized;
            hwndSource?.RemoveHook(HwndHook);
            hwnd = null;
        }

        private void AssociatedWindowSourceInitialized(object? sender, EventArgs e)
        {
            AddHwndHook();

            currentDpiRatio = DpiAware.GetScaleRatio(window);
            UpdateDpiScaling(currentDpiRatio, true);
        }

        private IntPtr HwndHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (message is 0x02E0)
            {
                RECT rect = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT))!;

                User32.SetWindowPosFlags flag =
                    User32.SetWindowPosFlags.DoNotChangeOwnerZOrder
                    | User32.SetWindowPosFlags.DoNotActivate
                    | User32.SetWindowPosFlags.IgnoreZOrder;
                User32.SetWindowPos(hWnd, IntPtr.Zero, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, flag);

                // we modified this fragment to correct the wrong behaviour
                double newDpiRatio = DpiAware.GetScaleRatio(window) * currentDpiRatio;
                if (newDpiRatio != currentDpiRatio)
                {
                    UpdateDpiScaling(newDpiRatio);
                }
            }

            return IntPtr.Zero;
        }

        private void UpdateDpiScaling(double newDpiRatio, bool useSacleCenter = false)
        {
            currentDpiRatio = newDpiRatio;
            Logger.LogStatic($"Set dpi scaling to {currentDpiRatio:p2}");
            Visual firstChild = (Visual)VisualTreeHelper.GetChild(window, 0);
            ScaleTransform transform;
            if (useSacleCenter)
            {
                double centerX = window.Left + (window.Width / 2);
                double centerY = window.Top + (window.Height / 2);

                transform = new ScaleTransform(currentDpiRatio, currentDpiRatio, centerX, centerY);
            }
            else
            {
                transform = new ScaleTransform(currentDpiRatio, currentDpiRatio);
            }

            firstChild.SetValue(Window.LayoutTransformProperty, transform);
        }
    }
}