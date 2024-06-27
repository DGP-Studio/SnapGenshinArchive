using CommunityToolkit.Mvvm.Input;
using DGP.Genshin.Control.GenshinElement;
using DGP.Genshin.DataModel;
using DGP.Genshin.DataModel.Achievement;
using DGP.Genshin.DataModel.Achievement.Decomposed;
using DGP.Genshin.DataModel.Character;
using DGP.Genshin.DataModel.GachaStatistic.Banner;
using DGP.Genshin.DataModel.Material;
using DGP.Genshin.Service.Abstraction.IntegrityCheck;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Core.Mvvm;
using Snap.Data.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DataModelWeapon = DGP.Genshin.DataModel.Weapon.Weapon;
using WeaponMaterial = DGP.Genshin.DataModel.Material.Weapon;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 元数据视图模型
    /// 存有各类共享物品数据
    /// </summary>
    [ViewModel(InjectAs.Singleton)]
    internal class MetadataViewModel : ObservableObject2
    {
        private const string CharactersJson = "characters.json";
        private const string DailyTalentsJson = "dailytalents.json";
        private const string DailyWeaponsJson = "dailyweapons.json";
        private const string WeaponsJson = "weapons.json";
        private const string GachaEventJson = "gachaevents.json";
        private const string AchieventsJson = "achievements.json";
        private const string AchieventGoalsJson = "achievementgoals.json";
        private const string DecomposedAchievementsJson = "decomposedachievements.json";

        private const string FolderPath = "Metadata\\";

        private Character? selectedCharacter;
        private DataModelWeapon? selectedWeapon;

        private ObservableCollection<Character> characters = null!;
        private ObservableCollection<DataModelWeapon> weapons = null!;
        private List<Talent> dailyTalents = null!;
        private List<WeaponMaterial> dailyWeapons = null!;
        private List<SpecificBanner> specificBanners = null!;
        private List<Achievement> achievements = null!;
        private List<AchievementGoal> achievementGoals = null!;
        private List<DecomposedAchievement> decomposedAchievements = null!;

        /// <summary>
        /// 构造一个新的元数据视图模型
        /// </summary>
        public MetadataViewModel()
        {
            CharacterInitializeCommand = new RelayCommand(() => { SelectedCharacter ??= Characters?.First(); });
            WeaponInitializeCommand = new RelayCommand(() => { SelectedWeapon ??= Weapons?.First(); });
            GachaSplashCommand = new RelayCommand(() =>
            {
                new CharacterGachaSplashWindow()
                {
                    Source = SelectedCharacter?.GachaSplash,
                    Owner = App.Current.MainWindow,
                }.ShowDialog();
            });
        }

        /// <summary>
        /// 角色集合
        /// </summary>
        [IntegrityAware]
        public ObservableCollection<Character> Characters
        {
            get => ProbeCollcetion(ref characters, CharactersJson, collection =>
                {
                    return new(collection
                            .OrderByDescending(i => i.Star)
                            .ThenBy(i => i.Element)
                            .ThenBy(i => i.Name));
                });
        }

        /// <summary>
        /// 武器集合
        /// </summary>
        [IntegrityAware]
        public ObservableCollection<DataModelWeapon> Weapons
        {
            get => ProbeCollcetion(ref weapons, WeaponsJson, collection =>
            {
                return new(collection
                        .OrderByDescending(i => i.Star)
                        .ThenBy(i => i.Type)
                        .ThenBy(i => i.Name));
            });
        }

        /// <summary>
        /// 日常天赋集合
        /// </summary>
        [IntegrityAware]
        public List<Talent> DailyTalents
        {
            get => ProbeCollcetion(ref dailyTalents, DailyTalentsJson);
        }

        /// <summary>
        /// 日常武器集合
        /// </summary>
        [IntegrityAware]
        public List<WeaponMaterial> DailyWeapons
        {
            get => ProbeCollcetion(ref dailyWeapons, DailyWeaponsJson);
        }

        /// <summary>
        /// 卡池信息集合
        /// </summary>
        public List<SpecificBanner> SpecificBanners
        {
            get => ProbeCollcetion(ref specificBanners, GachaEventJson, list =>
            {
                foreach (var banner in list)
                {
                    banner.UpStar5List = banner.UpStar5List!.Select(s =>
                    {
                        Primitive p = FindPrimitiveByName(s.Name)!;

                        return new DataModel.GachaStatistic.Item.StatisticItem()
                        {
                            StarUrl = p.Star,
                            Source = p.Source,
                            Name = p.Name,
                            Badge = p.GetBadge(),
                        };
                    }).ToList();

                    banner.UpStar4List = banner.UpStar4List!.Select(s =>
                    {
                        Primitive p = FindPrimitiveByName(s.Name)!;

                        return new DataModel.GachaStatistic.Item.StatisticItem()
                        {
                            StarUrl = p.Star,
                            Source = p.Source,
                            Name = p.Name,
                            Badge = p.GetBadge(),
                        };
                    }).ToList();
                }

                return list;
            });
        }

        /// <summary>
        /// 成就列表
        /// </summary>
        public List<Achievement> Achievements
        {
            get => ProbeCollcetion(ref achievements, AchieventsJson);
        }

        /// <summary>
        /// 成就大纲
        /// </summary>
        public List<AchievementGoal> AchievementGoals
        {
            get => ProbeCollcetion(ref achievementGoals, AchieventGoalsJson);
        }

        /// <summary>
        /// 分步成就
        /// </summary>
        public List<DecomposedAchievement> DecomposedAchievements
        {
            get => ProbeCollcetion(ref decomposedAchievements, DecomposedAchievementsJson);
        }

        /// <summary>
        /// 当前选择的角色
        /// </summary>
        public Character? SelectedCharacter
        {
            get => selectedCharacter;

            set => SetProperty(ref selectedCharacter, value);
        }

        /// <summary>
        /// 当前选择的武器
        /// </summary>
        public DataModelWeapon? SelectedWeapon
        {
            get => selectedWeapon;

            set => SetProperty(ref selectedWeapon, value);
        }

        /// <summary>
        /// 角色页面初始化命令
        /// </summary>
        public ICommand CharacterInitializeCommand { get; }

        /// <summary>
        /// 武器页面初始化命令
        /// </summary>
        public ICommand WeaponInitializeCommand { get; }

        /// <summary>
        /// 卡池立绘展示命令
        /// </summary>
        public ICommand GachaSplashCommand { get; }

        /// <summary>
        /// 根据名称在角色与武器集合中查找合适的元件
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>元件</returns>
        public Primitive? FindPrimitiveByName(string? name)
        {
            if (name is null)
            {
                return null;
            }

            return characters?.FirstOrDefault(c => c.Name == name) as Primitive
                ?? weapons?.FirstOrDefault(w => w.Name == name)
                ?? null;
        }

        /// <summary>
        /// 根据名称查找合适的url
        /// 仅限角色与武器
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>Url</returns>
        public string? FindSourceByName(string? name)
        {
            return FindPrimitiveByName(name)?.Source;
        }

        private T ProbeCollcetion<T>(ref T collection, string fileName, Func<T, T>? firstTimeFactory = null)
            where T : new()
        {
            if (collection is not null)
            {
                return collection;
            }
            else
            {
                string path = PathContext.Locate(FolderPath, fileName);
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    this.Log($"{fileName} loaded.");
                    collection ??= Json.ToObjectOrNew<T>(json);
                    if (firstTimeFactory != null)
                    {
                        collection = firstTimeFactory(collection);
                    }

                    return collection;
                }
                else
                {
                    throw Verify.FailOperation($"无法在 Metadata 文件夹中找到 {fileName}");
                }
            }
        }
    }
}