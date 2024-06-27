using DGP.Genshin.Service.Abstraction;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Data.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IState = DGP.Genshin.Service.Abstraction.IntegrityCheck.IIntegrityCheckService.IIntegrityCheckState;

namespace DGP.Genshin.Service
{
    /// <inheritdoc cref="IMetadataService"/>
    [Service(typeof(IMetadataService), InjectAs.Transient)]
    internal class MetadataService : IMetadataService
    {
        private const string MetaUrl = "https://metadata.snapgenshin.com/meta.json";
        private const string MetaUrlFormat = "https://metadata.snapgenshin.com/{0}.json";
        private const string MetaFolder = "Metadata";
        private const string MetaFile = "meta.json";

        /// <inheritdoc/>
        public bool IsMetaPresent
        {
            get => PathContext.FileExists(MetaFolder, MetaFile);
        }

        /// <inheritdoc/>
        public async Task<bool> TryEnsureDataNewestAsync(IProgress<IState>? progress, CancellationToken cancellationToken = default)
        {
            try
            {
                progress?.Report(new MetaState(1, 1, "检测元数据版本"));
                PathContext.CreateFolderOrIgnore(MetaFolder);

                Dictionary<string, string>? remoteVersions = await Json.FromWebsiteAsync<Dictionary<string, string>>(MetaUrl, cancellationToken);
                Dictionary<string, string> localVersions = Json.FromFileOrNew<Dictionary<string, string>>(PathContext.Locate(MetaFolder, MetaFile));

                Must.NotNull(remoteVersions!);

                int count = 0;
                foreach ((string file, string remoteVersion) in remoteVersions)
                {
                    bool shouldPass = false;

                    // 本地存在版本 且 远程版本不大于本地版本
                    if (localVersions.TryGetValue(file, out string? localVersion)
                        && (new Version(remoteVersion) <= new Version(localVersion)))
                    {
                        this.Log($"Skip {file} of version {remoteVersion}.");
                        shouldPass = true;

                        if (!PathContext.FileExists(MetaFolder, $"{file}.json"))
                        {
                            shouldPass = false;
                        }
                    }

                    if (shouldPass)
                    {
                        progress?.Report(new MetaState(++count, remoteVersions.Count, $"{file}.json"));
                    }
                    else
                    {
                        this.Log($"Download {file} of version {remoteVersion}.");
                        string destinationPath = PathContext.Locate(MetaFolder, $"{file}.json");
                        await Json.WebsiteToFileAsync(string.Format(MetaUrlFormat, file), destinationPath, cancellationToken);
                        progress?.Report(new MetaState(++count, remoteVersions.Count, $"{file}.json"));
                    }
                }

                Json.ToFile(PathContext.Locate(MetaFolder, MetaFile), remoteVersions);
                return true;
            }
            catch (Exception ex)
            {
                this.Log(ex);
            }

            return false;
        }

        public class MetaState : IState
        {
            /// <summary>
            /// 构造新的进度实例
            /// </summary>
            /// <param name="count">当前个数</param>
            /// <param name="totalCount">总个数</param>
            /// <param name="ks">当前检查完成的源</param>
            public MetaState(int count, int totalCount, string info)
            {
                CurrentCount = count;
                TotalCount = totalCount;
                Info = info;
            }

            /// <summary>
            /// 当前个数
            /// </summary>
            public int CurrentCount { get; init; }

            /// <summary>
            /// 总个数
            /// </summary>
            public int TotalCount { get; init; }

            /// <summary>
            /// 展示信息
            /// </summary>
            public string? Info { get; init; }
        }
    }
}