using Newtonsoft.Json;
using Snap.Data.Primitive;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    public class Consumption : Observable
    {
        private List<ConsumeItem>? avatarConsume;
        private List<ConsumeItem>? avatarSkillConsume;
        private List<ConsumeItem>? weaponConsume;
        private List<ReliquaryConsumeItem>? reliquaryConsume;

        [JsonProperty("avatar_consume")]
        public List<ConsumeItem>? AvatarConsume
        {
            get => avatarConsume;

            set => Set(ref avatarConsume, value);
        }
        [JsonProperty("avatar_skill_consume")]
        public List<ConsumeItem>? AvatarSkillConsume
        {
            get => avatarSkillConsume;

            set => Set(ref avatarSkillConsume, value);
        }
        [JsonProperty("weapon_consume")]
        public List<ConsumeItem>? WeaponConsume
        {
            get => weaponConsume;

            set => Set(ref weaponConsume, value);
        }
        [JsonProperty("reliquary_consume")]
        public List<ReliquaryConsumeItem>? ReliquaryConsume
        {
            get => reliquaryConsume;

            set => Set(ref reliquaryConsume, value);
        }
    }
}
