using DGP.Genshin.Service.Abstraction.Setting;

namespace DGP.Genshin.Service.Abstraction.Launching
{
    /// <summary>
    /// 封装启动参数
    /// </summary>
    public class LaunchOption
    {
        /// <summary>
        /// 是否无边框
        /// </summary>
        public bool IsBorderless { get; set; }

        /// <summary>
        /// 是否全屏，全屏时无边框设置将被覆盖
        /// </summary>
        public bool IsFullScreen { get; set; }

        /// <summary>
        /// 是否启用解锁帧率
        /// </summary>
        public bool UnlockFPS { get; set; }

        /// <summary>
        /// 目标帧率
        /// </summary>
        public int TargetFPS { get; set; }

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public int ScreenWidth { get; set; }

        /// <summary>
        /// 窗口高度
        /// </summary>
        public int ScreenHeight { get; set; }

        /// <summary>
        /// 使用当前的设置项创建启动参数
        /// </summary>
        /// <returns>当前的设置项创建启动参数</returns>
        public static LaunchOption FromCurrentSettings()
        {
            return new()
            {
                IsBorderless = Setting2.IsBorderless.Get(),
                IsFullScreen = Setting2.IsFullScreen.Get(),
                UnlockFPS = App.IsElevated && Setting2.UnlockFPS.Get(),
                TargetFPS = (int)Setting2.TargetFPS.Get(),
                ScreenWidth = (int)Setting2.ScreenWidth.Get(),
                ScreenHeight = (int)Setting2.ScreenHeight.Get(),
            };
        }
    }
}