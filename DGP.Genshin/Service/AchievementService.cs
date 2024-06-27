using DGP.Genshin.DataModel.Achievement;
using DGP.Genshin.DataModel.Achievement.CocoGoat;
using DGP.Genshin.DataModel.Achievement.UIAF;
using DGP.Genshin.Service.Abstraction.Achievement;
using Newtonsoft.Json;
using Snap.Core.DependencyInjection;
using Snap.Data.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DGP.Genshin.Service
{
    /// <inheritdoc cref="IAchievementService"/>
    [Service(typeof(IAchievementService), InjectAs.Transient)]
    internal class AchievementService : IAchievementService
    {
        private const string AchievementsFileName = "achievements.json";
        private const string AchievementStepsFileName = "achievementsteps.json";

        /// <inheritdoc/>
        public List<IdTime> GetCompletedItems()
        {
            return Json.FromFileOrNew<List<IdTime>>(PathContext.Locate(AchievementsFileName));
        }

        /// <inheritdoc/>
        public List<IdSteps> GetCompletedSteps()
        {
            return Json.FromFileOrNew<List<IdSteps>>(PathContext.Locate(AchievementStepsFileName));
        }

        /// <inheritdoc/>
        public void SaveItems(ObservableCollection<Achievement> achievements)
        {
            // completed items
            IEnumerable<IdTime> idTimes = achievements
                .Where(a => a.IsCompleted)
                .Select(a => new IdTime(a.Id, a.CompleteDateTime));
            Json.ToFile(PathContext.Locate(AchievementsFileName), idTimes);

            // completed steps
            IEnumerable<IdSteps> idStates = achievements
                .Where(a => a.Decomposition is not null)
                .Select(a => new IdSteps(a.Id, a.Decomposition!.Steps!.Select(s => s.IsCompleted).ToList()));
            Json.ToFile(PathContext.Locate(AchievementStepsFileName), idStates);
        }

        /// <inheritdoc/>
        public IEnumerable<IdTime>? TryGetImportData(ImportAchievementSource source, string argument)
        {
            try
            {
                switch (source)
                {
                    case ImportAchievementSource.Cocogoat:
                        {
                            CocoGoatUserData? data = Json.FromFile<CocoGoatUserData>(argument);
                            if (data?.Value?.Achievements is List<CocoGoatAchievement> achievements)
                            {
                                return achievements
                                    .Select(a => new IdTime(a.Id, a.Date));
                            }

                            break;
                        }

                    case ImportAchievementSource.UIAF:
                        {
                            UIAF? data = Json.ToObject<UIAF>(argument);
                            if (data?.List is List<UIAFItem> achievements)
                            {
                                return achievements

                                    // WHERE achievement: status invalid && time 0001/01/01 00:00:00
                                    // which indicates a obsolete achievement for UIAF 1.1.
                                    // Meanwhile we want to compact with UIAF 1.0 where status will be 0 but not for time.
                                    .Where(a => ((int)a.Status >= 2) || (a.Status == 0 && a.TimeStamp != -62135596800))
                                    .Select(a => new IdTime(a.Id, DateTime.UnixEpoch.AddSeconds(a.TimeStamp)));
                            }

                            break;
                        }

                    default:
                        break;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<IdTime>? TryGetImportData(string dataString)
        {
            try
            {
                IEnumerable<IdTimeStamp>? idTimeStamps = Json.ToObject<IEnumerable<IdTimeStamp>>(dataString);
                return idTimeStamps?
                    .Select(ts => new IdTime(ts.Id, DateTime.UnixEpoch.AddSeconds(ts.TimeStamp)));
            }
            catch (JsonReaderException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (JsonSerializationException)
            {
                return null;
            }
        }
    }
}