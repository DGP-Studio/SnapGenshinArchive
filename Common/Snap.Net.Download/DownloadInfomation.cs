namespace Snap.Net.Download
{
    /// <summary>
    /// 表示下载信息
    /// </summary>
    public record DownloadInfomation
    {
        /// <summary>
        /// 构造一个新的下载信息
        /// </summary>
        /// <param name="bytesReceived">当前收到的字节</param>
        /// <param name="totalSize">总字节数</param>
        public DownloadInfomation(long bytesReceived, long totalSize)
        {
            BytesReceived = bytesReceived;
            TotalSize = totalSize;
        }

        /// <summary>
        /// 当前收到的字节
        /// </summary>
        public long BytesReceived { get; }

        /// <summary>
        /// 总字节数
        /// </summary>
        public long TotalSize { get; }

        /// <summary>
        /// 进度百分比
        /// </summary>
        public double Percent
        {
            get => (double)BytesReceived / TotalSize;
        }

        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsDownloading
        {
            get => Percent < 1;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            double megaBytesReceived = BytesReceived * 1.0 / 1024 / 1024;
            double megaTotalSize = TotalSize * 1.0 / 1024 / 1024;

            return $@"{Percent:P2} - {megaBytesReceived:F2}MB / {megaTotalSize:F2}MB";
        }
    }
}