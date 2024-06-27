using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.DataModel.Character;
using DGP.Genshin.DataModel.Material;
using DGP.Genshin.Factory.Abstraction;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using Snap.Core.Mvvm;
using Snap.Data.Primitive;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataModelWeapon = DGP.Genshin.DataModel.Weapon.Weapon;
using WeaponMaterial = DGP.Genshin.DataModel.Material.Weapon;

namespace DGP.Genshin.ViewModel
{
    /// <summary>
    /// 日常材料服务
    /// </summary>
    [ViewModel(InjectAs.Singleton)]
    internal class DailyViewModel : ObservableObject2, ISupportCancellation
    {
        private const string MondstadtIcon = "https://upload-bbs.mihoyo.com/game_record/genshin/city_icon/UI_ChapterIcon_Mengde.png";
        private const string LiyueIcon = "https://upload-bbs.mihoyo.com/game_record/genshin/city_icon/UI_ChapterIcon_Liyue.png";
        private const string InazumaIcon = "https://upload-bbs.mihoyo.com/game_record/genshin/city_icon/UI_ChapterIcon_Daoqi.png";
        private const string SumeruIcon = "https://upload-bbs.mihoyo.com/game_record/genshin/city_icon/UI_ChapterIcon_Sumeru.png";

        private readonly MetadataViewModel metadata;

        private IList<City>? cities;

        /// <summary>
        /// 构造一个新的日常视图模型
        /// </summary>
        /// <param name="metadataViewModel">元数据视图模型</param>
        /// <param name="asyncRelayCommandFactory">异步命令工厂</param>
        public DailyViewModel(MetadataViewModel metadataViewModel, IAsyncRelayCommandFactory asyncRelayCommandFactory)
        {
            metadata = metadataViewModel;

            OpenUICommand = asyncRelayCommandFactory.Create(OpenUIAsync);
        }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public IList<City>? Cities { get => cities; set => SetProperty(ref cities, value); }

        /// <summary>
        /// 打开界面触发的命令
        /// </summary>
        public ICommand OpenUICommand { get; }

        private async Task OpenUIAsync()
        {
            try
            {
                await Task.Delay(500, CancellationToken);
                BuildCities();
            }
            catch (TaskCanceledException)
            {
                this.Log("Open UI cancelled");
            }
        }

        private void BuildCities()
        {
            DateTime now = DateTime.UtcNow + TimeSpan.FromHours(4);
            DayOfWeek dayOfWeek = now.DayOfWeek;

            List<Indexed<Talent, Character>> mondstadtCharacter = new()
            {
                IndexedFromTalentName(Talent.Freedom, dayOfWeek, 1),
                IndexedFromTalentName(Talent.Resistance, dayOfWeek, 2),
                IndexedFromTalentName(Talent.Ballad, dayOfWeek, 0),
            };
            List<Indexed<WeaponMaterial, DataModelWeapon>> mondstadtWeapon = new()
            {
                IndexedFromMaterialName(WeaponMaterial.Decarabian, dayOfWeek, 1),
                IndexedFromMaterialName(WeaponMaterial.BorealWolf, dayOfWeek, 2),
                IndexedFromMaterialName(WeaponMaterial.DandelionGladiator, dayOfWeek, 0),
            };
            City mondstadt = new("蒙德", MondstadtIcon, mondstadtCharacter, mondstadtWeapon);

            List<Indexed<Talent, Character>> liyueCharacter = new()
            {
                IndexedFromTalentName(Talent.Prosperity, dayOfWeek, 1),
                IndexedFromTalentName(Talent.Diligence, dayOfWeek, 2),
                IndexedFromTalentName(Talent.Gold, dayOfWeek, 0),
            };
            List<Indexed<WeaponMaterial, DataModelWeapon>> liyueWeapon = new()
            {
                IndexedFromMaterialName(WeaponMaterial.Guyun, dayOfWeek, 1),
                IndexedFromMaterialName(WeaponMaterial.MistVeiled, dayOfWeek, 2),
                IndexedFromMaterialName(WeaponMaterial.Aerosiderite, dayOfWeek, 0),
            };
            City liyue = new("璃月", LiyueIcon, liyueCharacter, liyueWeapon);

            List<Indexed<Talent, Character>> inazumaCharacter = new()
            {
                IndexedFromTalentName(Talent.Transience, dayOfWeek, 1),
                IndexedFromTalentName(Talent.Elegance, dayOfWeek, 2),
                IndexedFromTalentName(Talent.Light, dayOfWeek, 0),
            };
            List<Indexed<WeaponMaterial, DataModelWeapon>> inazumaWeapon = new()
            {
                IndexedFromMaterialName(WeaponMaterial.DistantSea, dayOfWeek, 1),
                IndexedFromMaterialName(WeaponMaterial.Narukami, dayOfWeek, 2),
                IndexedFromMaterialName(WeaponMaterial.Mask, dayOfWeek, 0),
            };
            City inazuma = new("稻妻", InazumaIcon, inazumaCharacter, inazumaWeapon);

            Cities = new List<City>()
            {
                mondstadt,
                liyue,
                inazuma,
            };
        }

        private Indexed<Talent, Character> IndexedFromTalentName(string talentName, DayOfWeek dayOfWeek, int position)
        {
            return new(
                metadata.DailyTalents
                    .First(t => t.Source == talentName)
                    .SetAvailability(dayOfWeek == DayOfWeek.Sunday || (int)dayOfWeek % 3 == position),
                metadata.Characters
                    .Where(c => c.Talent!.Source == talentName)
                    .ToList());
        }

        private Indexed<WeaponMaterial, DataModelWeapon> IndexedFromMaterialName(string materialName, DayOfWeek dayOfWeek, int position)
        {
            return new(
                metadata.DailyWeapons
                    .First(t => t.Source == materialName)
                    .SetAvailability(dayOfWeek == DayOfWeek.Sunday || (int)dayOfWeek % 3 == position),
                metadata.Weapons
                    .Where(c => c.Ascension!.Source == materialName)
                    .ToList());
        }

        /// <summary>
        /// 表示一个国家的日常信息
        /// </summary>
        internal record City
        {
            /// <summary>
            /// 构造一个新的国家实例
            /// </summary>
            /// <param name="name">城市名称</param>
            /// <param name="source">城市图片Url</param>
            /// <param name="characters">角色列表</param>
            /// <param name="weapons">武器列表</param>
            public City(string name, string source, IList<Indexed<Talent, Character>> characters, IList<Indexed<WeaponMaterial, DataModelWeapon>> weapons)
            {
                Name = name;
                Source = source;
                Characters = characters;
                Weapons = weapons;
            }

            /// <summary>
            /// 城市名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 城市图片Url
            /// </summary>
            public string Source { get; set; }

            /// <summary>
            /// 角色列表
            /// </summary>
            public IList<Indexed<Talent, Character>> Characters { get; set; }

            /// <summary>
            /// 武器列表
            /// </summary>
            public IList<Indexed<WeaponMaterial, DataModelWeapon>> Weapons { get; set; }
        }
    }
}