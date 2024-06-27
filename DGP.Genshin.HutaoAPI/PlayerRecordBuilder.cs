using DGP.Genshin.HutaoAPI.PostModel;
using DGP.Genshin.MiHoYoAPI.Record.Avatar;
using DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss;
using Microsoft;
using Snap.Data.Utility;
using System.Collections.Generic;
using System.Linq;

namespace DGP.Genshin.HutaoAPI
{
    /// <summary>
    /// 玩家记录建造器
    /// </summary>
    internal static class PlayerRecordBuilder
    {
        /// <summary>
        /// 建造玩家记录
        /// </summary>
        /// <param name="uid">玩家的uid</param>
        /// <param name="detailAvatars">角色详情信息</param>
        /// <param name="spiralAbyss">深渊信息</param>
        /// <returns>玩家记录</returns>
        internal static PlayerRecord BuildPlayerRecord(string uid, DetailedAvatarWrapper detailAvatars, SpiralAbyss spiralAbyss)
        {
            Requires.NotNull(detailAvatars.Avatars!, nameof(detailAvatars.Avatars));
            List<PlayerAvatar> playerAvatars = detailAvatars.Avatars
                .Select(avatar => new PlayerAvatar(
                    avatar.Id,
                    avatar.Level,
                    avatar.ActivedConstellationNum,
                    BuildAvatarWeapon(avatar.Weapon),
                    BuildAvatarReliquarySets(avatar.Reliquaries)))
                .ToList();

            Requires.NotNull(spiralAbyss.Floors!, nameof(spiralAbyss.Floors));
            List<PlayerSpiralAbyssLevel> playerSpiralAbyssLevels = spiralAbyss.Floors
                .SelectMany(f => f.Levels!, (f, level) => new FloorIndexedLevel(f.Index, level))
                .Select(indexedLevel => new PlayerSpiralAbyssLevel(
                    indexedLevel.FloorIndex,
                    indexedLevel.Level.Index,
                    indexedLevel.Level.Star,
                    indexedLevel.Level.Battles!
                    .Select(b => new PlayerSpiralAbyssBattle(
                        b.Index,
                        b.Avatars!.Select(a => a.Id).ToList()))
                    .ToList()))
                .ToList();

            RankInfo? damage = null;
            if (spiralAbyss.DamageRank!.Count >= 1)
            {
                Rank? rank = spiralAbyss.DamageRank[0];
                damage = new RankInfo { AvatarId = rank.AvatarId, Value = rank.Value };
            }

            RankInfo? takeDamage = null;
            if (spiralAbyss.TakeDamageRank!.Count >= 1)
            {
                Rank? rank = spiralAbyss.TakeDamageRank[0];
                takeDamage = new RankInfo { AvatarId = rank.AvatarId, Value = rank.Value };
            }

            PlayerRecord playerRecord = new(uid, playerAvatars, playerSpiralAbyssLevels)
            {
                DamageMost = damage,
                TakeDamageMost = takeDamage,
            };

            return playerRecord;
        }

        /// <summary>
        /// 建造角色的武器信息
        /// </summary>
        /// <param name="weapon">武器</param>
        /// <returns>角色的武器</returns>
        private static AvatarWeapon BuildAvatarWeapon(Weapon? weapon)
        {
            Requires.NotNull(weapon!, nameof(weapon));
            return new(weapon.Id, weapon.Level, weapon.AffixLevel);
        }

        /// <summary>
        /// 建造角色的圣遗物信息
        /// </summary>
        /// <param name="reliquaries">圣遗物列表</param>
        /// <returns>角色的圣遗物信息</returns>
        private static List<AvatarReliquarySet> BuildAvatarReliquarySets(List<Reliquary>? reliquaries)
        {
            Requires.NotNull(reliquaries!, nameof(reliquaries));
            CounterInt32<int> reliquarySetCounter = new();
            foreach (Reliquary reliquary in reliquaries)
            {
                if (reliquary.ReliquarySet is not null)
                {
                    reliquarySetCounter.Increase(reliquary.ReliquarySet.Id);
                }
            }

            // 含有2件套以上的套装
            return reliquarySetCounter.Keys.Any(k => k >= 2)
                ? reliquarySetCounter.Select(kvp => new AvatarReliquarySet(kvp)).ToList()
                : new();
        }

        private record FloorIndexedLevel(int FloorIndex, Level Level);
    }
}