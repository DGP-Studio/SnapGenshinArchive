using DGP.Genshin.Control.Infrastructure.CachedImage;
using DGP.Genshin.DataModel;
using DGP.Genshin.Service.Abstraction;
using DGP.Genshin.Service.Abstraction.IntegrityCheck;
using DGP.Genshin.Service.Abstraction.Setting;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Data.Primitive;
using Snap.Reflection;
using Snap.Threading;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IState = DGP.Genshin.Service.Abstraction.IntegrityCheck.IIntegrityCheckService.IIntegrityCheckState;
#if RELEASE
using DGP.Genshin.Core.Notification;
using Microsoft.Toolkit.Uwp.Notifications;
#endif

namespace DGP.Genshin.Service
{
    /// <summary>
    /// 完整性检查服务的默认实现
    /// </summary>
    [Service(typeof(IIntegrityCheckService), InjectAs.Transient)]
    internal class IntegrityCheckService : IIntegrityCheckService
    {
        private readonly MetadataViewModel metadataViewModel;
        private readonly IMetadataService metadataService;

        /// <summary>
        /// 累计检查的个数
        /// </summary>
        private int cumulatedCount;

        /// <summary>
        /// 构造一个新的完整性检测服务
        /// </summary>
        /// <param name="metadataViewModel">元数据视图模型</param>
        public IntegrityCheckService(MetadataViewModel metadataViewModel, IMetadataService metadataService)
        {
            this.metadataViewModel = metadataViewModel;
            this.metadataService = metadataService;
        }

        /// <inheritdoc/>
        public WorkWatcher IntegrityChecking { get; } = new(false);

        /// <inheritdoc/>
        public async Task CheckMetadataIntegrityAsync(IProgress<IState> progress)
        {
            this.Log("Integrity Check Start");
            using (IntegrityChecking.Watch())
            {
#if RELEASE
                // 更新后或首次启动时
                // 每次更新后至少检查一次元数据版本
                if (App.Current.Version > Setting2.AppVersion || !metadataService.IsMetaPresent)
                {
                    if (!await metadataService.TryEnsureDataNewestAsync(progress))
                    {
                        new ToastContentBuilder()
                            .AddText("更新元数据失败")
                            .AddText("应用可能会意外崩溃")
                            .SafeShow();
                    }
                }
#endif
                int totalCount = GetTotalCount(metadataViewModel);
                await Task.WhenAll(BuildIntegrityTasks(metadataViewModel, totalCount, progress));
                this.Log($"Integrity Check Complete with {totalCount} entries");
            }
        }

        private int GetTotalCount(MetadataViewModel metadata)
        {
            int totalCount = 0;
            metadata.ForEachPropertyWithAttribute<IntegrityAwareAttribute>((prop, _) =>
            {
                totalCount += prop.GetPropertyValueByName<int>("Count");
            });
            return totalCount;
        }

        /// <summary>
        /// 构造检查任务
        /// </summary>
        /// <param name="metadata">元数据视图模型</param>
        /// <param name="totalCount">总个数</param>
        /// <param name="progress">进度</param>
        /// <returns>等待执行的检查任务</returns>
        private List<Task> BuildIntegrityTasks(MetadataViewModel metadata, int totalCount, IProgress<IState> progress)
        {
            List<Task> tasks = new();

            metadata.ForEachPropertyWithAttribute<IEnumerable<KeySource>, IntegrityAwareAttribute>((keySources, attr) =>
            {
                tasks.Add(CheckIntegrityAsync(keySources, totalCount, progress));
            });
            return tasks;
        }

        /// <summary>
        /// 检查单个集合的Source
        /// </summary>
        /// <typeparam name="T">包含的物品类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="totalCount">总个数</param>
        /// <param name="progress">进度</param>
        private async Task CheckIntegrityAsync<T>(IEnumerable<T>? collection, int totalCount, IProgress<IState> progress)
            where T : KeySource
        {
            if (collection is not null)
            {
                await collection.ParallelForEachAsync(async t =>
                {
                    if (!FileCache.Exists(t.Source))
                    {
                        using MemoryStream? memoryStream = await FileCache.HitAsync(t.Source);
                    }

                    progress.Report(new IntegrityState(++cumulatedCount, totalCount, t));
                });
            }
        }

        /// <summary>
        /// <inheritdoc cref="IState"/>
        /// </summary>
        public class IntegrityState : IState
        {
            /// <summary>
            /// 构造新的进度实例
            /// </summary>
            /// <param name="count">当前个数</param>
            /// <param name="totalCount">总个数</param>
            /// <param name="ks">当前检查完成的源</param>
            public IntegrityState(int count, int totalCount, KeySource? ks)
            {
                CurrentCount = count;
                TotalCount = totalCount;
                Info = ks?.Source?.ToShortFileName();
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