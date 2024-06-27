using DGP.Genshin.DataModel.HutaoAPI;
using DGP.Genshin.HutaoAPI;
using DGP.Genshin.HutaoAPI.GetModel;
using DGP.Genshin.HutaoAPI.PostModel;
using DGP.Genshin.MiHoYoAPI.Response;
using DGP.Genshin.Service.Abstraction;
using Snap.Core.DependencyInjection;
using Snap.Data.Primitive;
using Snap.Extenion.Enumerable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Service
{
    /// <inheritdoc/>
    [Service(typeof(IHutaoStatisticService), InjectAs.Transient)]
    internal class HutaoStatisticService : IHutaoStatisticService
    {
        private readonly PlayerRecordClient playerRecordClient = new();

        private IEnumerable<HutaoItem> avatarMap = null!;
        private IEnumerable<HutaoItem> weaponMap = null!;
        private IEnumerable<HutaoItem> reliquaryMap = null!;

        private IEnumerable<AvatarParticipation> avatarParticipation2s = null!;
        private IEnumerable<AvatarConstellationNum> avatarConstellationNums = null!;
        private IEnumerable<TeamCollocation> teamCollocations = null!;
        private IEnumerable<WeaponUsage> weaponUsages = null!;
        private IEnumerable<AvatarReliquaryUsage> avatarReliquaryUsages = null!;
        private IEnumerable<TeamCombination2> teamCombinations = null!;

        /// <inheritdoc/>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await playerRecordClient.InitializeAsync(cancellationToken);

            avatarMap = await playerRecordClient.GetAvatarMapAsync(cancellationToken);
            weaponMap = await playerRecordClient.GetWeaponMapAsync(cancellationToken);
            reliquaryMap = await playerRecordClient.GetReliquaryMapAsync(cancellationToken);

            avatarParticipation2s = await playerRecordClient.GetAvatarParticipation2sAsync(cancellationToken);
            avatarConstellationNums = await playerRecordClient.GetAvatarConstellationsAsync(cancellationToken);
            teamCollocations = await playerRecordClient.GetTeamCollocationsAsync(cancellationToken);
            weaponUsages = await playerRecordClient.GetAvatarWeaponUsagesAsync(cancellationToken);
            avatarReliquaryUsages = await playerRecordClient.GetAvatarReliquaryUsagesAsync(cancellationToken);
            teamCombinations = await playerRecordClient.GetTeamCombinations2Async(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Overview?> GetOverviewAsync(CancellationToken cancellationToken = default)
        {
            return await playerRecordClient.GetOverviewAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> GetPeriodUploadedAsync(string uid, CancellationToken cancellationToken = default)
        {
            return await playerRecordClient.CheckPeriodRecordUploadedAsync(uid, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Two<Item<Rank>>?> GetRankAsync(string uid, CancellationToken cancellationToken = default)
        {
            RankWrapper? rank = await playerRecordClient.GetRankAsync(uid, cancellationToken);
            if (rank == null || rank.Damage == null || rank.TakeDamage == null)
            {
                return null;
            }

            Item<Rank> damage = new(avatarMap.First(a => a.Id == rank.Damage.AvatarId), rank.Damage);
            Item<Rank> takeDamage = new(avatarMap.First(a => a.Id == rank.TakeDamage.AvatarId), rank.TakeDamage);

            return new Two<Item<Rank>>(damage, takeDamage);
        }

        /// <inheritdoc/>
        public IList<Indexed<int, Item<double>>> GetAvatarParticipation2s()
        {
            List<Indexed<int, Item<double>>> avatarParticipationResults = new();

            // 保证 12层在前
            foreach (AvatarParticipation avatarParticipation in avatarParticipation2s.OrderByDescending(x => x.Floor))
            {
                IList<Item<double>> result = avatarParticipation.AvatarUsage
                    .Join(
                        avatarMap,
                        rate => rate.Id,
                        avatar => avatar.Id,
                        (rate, avatar) => new Item<double>(avatar.Id, avatar.Name, avatar.Url, rate.Value))
                    .OrderByDescending(x => x.Value)
                    .ToList();

                avatarParticipationResults
                    .Add(new Indexed<int, Item<double>>(avatarParticipation.Floor, result));
            }

            return avatarParticipationResults;
        }

        /// <inheritdoc/>
        public IList<Rate<Item<IList<NamedValue<double>>>>> GetAvatarConstellations()
        {
            List<Rate<Item<IList<NamedValue<double>>>>> avatarConstellationsResults = new();
            foreach (AvatarConstellationNum avatarConstellationNum in avatarConstellationNums)
            {
                HutaoItem? matched = avatarMap.FirstOrDefault(x => x.Id == avatarConstellationNum.Avatar);
                if (matched != null)
                {
                    IList<NamedValue<double>> result = avatarConstellationNum.Rate
                        .OrderBy(rate => rate.Id)
                        .Select(rate => new NamedValue<double>($"{rate.Id} 命", rate.Value))
                        .ToList();

                    avatarConstellationsResults.Add(new()
                    {
                        Id = new(matched.Id, matched.Name, matched.Url, result),
                        Value = avatarConstellationNum.HoldingRate,
                    });
                }
            }

            return avatarConstellationsResults
                .OrderByDescending(x => x.Id!.Id)
                .ToList();
        }

        /// <inheritdoc/>
        public IList<Item<IList<Item<double>>>> GetTeamCollocations()
        {
            List<Item<IList<Item<double>>>> teamCollocationsResults = new();
            foreach (TeamCollocation teamCollocation in teamCollocations)
            {
                HutaoItem? matched = avatarMap.FirstOrDefault(x => x.Id == teamCollocation.Avatar);
                if (matched != null)
                {
                    IEnumerable<Item<double>> result = teamCollocation.Collocations
                    .Join(
                        avatarMap.DistinctBy(a => a.Id),
                        rate => rate.Id,
                        avatar => avatar.Id,
                        (rate, avatar) => new Item<double>(avatar.Id, avatar.Name, avatar.Url, rate.Value));

                    teamCollocationsResults
                        .Add(new Item<IList<Item<double>>>(
                            matched.Id,
                            matched.Name,
                            matched.Url,
                            result.OrderByDescending(x => x.Value).ToList()));
                }
            }

            return teamCollocationsResults
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        /// <inheritdoc/>
        public IList<Item<IList<Item<double>>>> GetWeaponUsages()
        {
            List<Item<IList<Item<double>>>> weaponUsagesResults = new();
            foreach (WeaponUsage weaponUsage in weaponUsages)
            {
                HutaoItem? matchedAvatar = avatarMap.FirstOrDefault(x => x.Id == weaponUsage.Avatar);
                if (matchedAvatar != null)
                {
                    IEnumerable<Item<double>> result = weaponUsage.Weapons
                        .Join(
                            weaponMap,
                            rate => rate.Id,
                            weapon => weapon.Id,
                            (rate, weapon) => new Item<double>(weapon.Id, weapon.Name, weapon.Url, rate.Value));

                    weaponUsagesResults
                        .Add(new Item<IList<Item<double>>>(
                            matchedAvatar.Id,
                            matchedAvatar.Name,
                            matchedAvatar.Url,
                            result.OrderByDescending(x => x.Value).ToList()));
                }
            }

            return weaponUsagesResults
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        /// <inheritdoc/>
        public IList<Item<IList<NamedValue<Rate<IList<Item<int>>>>>>> GetReliquaryUsages()
        {
            List<Item<IList<NamedValue<Rate<IList<Item<int>>>>>>> reliquaryUsagesResults = new();
            foreach (AvatarReliquaryUsage reliquaryUsage in avatarReliquaryUsages)
            {
                HutaoItem? matchedAvatar = avatarMap.FirstOrDefault(x => x.Id == reliquaryUsage.Avatar);
                if (matchedAvatar != null)
                {
                    List<NamedValue<Rate<IList<Item<int>>>>> result = new();

                    foreach (Rate<string> usage in reliquaryUsage.ReliquaryUsage)
                    {
                        List<Item<int>> relicList = new();
                        StringBuilder nameBuilder = new();
                        string[] relicWithCountArray = usage.Id!.Split(';');
                        foreach (string? relicAndCount in relicWithCountArray)
                        {
                            // 0 id 1 count
                            string[]? relicSetIdAndCount = relicAndCount.Split('-');
                            HutaoItem? matchedRelic = reliquaryMap.FirstOrDefault(x => x.Id == int.Parse(relicSetIdAndCount[0]));
                            if (matchedRelic != null)
                            {
                                string count = relicSetIdAndCount[1];
                                nameBuilder.Append($"{count}×{matchedRelic.Name} ");
                                relicList.Add(new Item<int>(matchedRelic.Id, matchedRelic.Name, matchedRelic.Url, int.Parse(count)));
                            }
                        }

                        if (nameBuilder.Length > 0)
                        {
                            Rate<IList<Item<int>>> rate = new() { Id = relicList, Value = usage.Value };

                            // remove last space
                            NamedValue<Rate<IList<Item<int>>>> namedValue = new(nameBuilder.ToString()[0..^1], rate);
                            result.Add(namedValue);
                        }
                    }

                    reliquaryUsagesResults
                        .Add(new Item<IList<NamedValue<Rate<IList<Item<int>>>>>>(
                            matchedAvatar.Id,
                            matchedAvatar.Name,
                            matchedAvatar.Url,
                            result));
                }
            }

            return reliquaryUsagesResults
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        /// <inheritdoc/>
        public IList<Indexed<int, Rate<Two<IList<HutaoItem>>>>> GetTeamCombinations()
        {
            List<Indexed<int, Rate<Two<IList<HutaoItem>>>>> teamCombinationResults = new();
            IEnumerable<TeamCombination2> reoderded = teamCombinations
                .OrderByDescending(x => x.Floor);

            foreach (TeamCombination2 temaCombination in reoderded)
            {
                IList<Rate<Two<IList<HutaoItem>>>> teamRates = temaCombination.Teams
                .Select(team => new Rate<Two<IList<HutaoItem>>>
                {
                    Value = team.Value,
                    Id = new(
                        team.Id!.GetUp().Select(id => avatarMap.FirstOrDefault(a => a.Id == id)).NotNull().ToList(),
                        team.Id!.GetDown().Select(id => avatarMap.FirstOrDefault(a => a.Id == id)).NotNull().ToList()),
                })
                .ToList();

                teamCombinationResults
                    .Add(new Indexed<int, Rate<Two<IList<HutaoItem>>>>(
                        temaCombination.Floor,
                        teamRates.OrderByDescending(x => x.Value).ToList()));
            }

            return teamCombinationResults;
        }

        /// <inheritdoc/>
        public async Task GetAllRecordsAndUploadAsync(string cookie, Func<PlayerRecord, Task<bool>> confirmFunc, Func<Response, Task> resultAsyncFunc, CancellationToken cancellationToken = default)
        {
            await playerRecordClient.GetAllRecordsAndUploadAsync(cookie, confirmFunc, resultAsyncFunc, cancellationToken);
        }
    }
}