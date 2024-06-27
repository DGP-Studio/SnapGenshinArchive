using Snap.Net.Download;

namespace DGP.Genshin.Message
{
    /// <summary>
    /// 更新进度消息
    /// </summary>
    public class UpdateProgressedMessage
    {
        private static UpdateProgressedMessage? defaultValue;

        /// <summary>
        /// 构造一个新的更新消息
        /// </summary>
        /// <param name="infomation">下载信息</param>
        public UpdateProgressedMessage(DownloadInfomation infomation)
        {
            Value = infomation.Percent;
            ValueString = infomation.ToString();
            IsDownloading = infomation.IsDownloading;
        }

        private UpdateProgressedMessage(double value, string valueString, bool isDownloading)
        {
            Value = value;
            ValueString = valueString;
            IsDownloading = isDownloading;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public static UpdateProgressedMessage Default
        {
            get
            {
                defaultValue ??= new UpdateProgressedMessage(0, string.Empty, false);
                return defaultValue;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// 值字符串
        /// </summary>
        public string ValueString { get; }

        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsDownloading { get; }
    }
}