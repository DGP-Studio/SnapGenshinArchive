using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    public class Weapon : Calculable
    {
        [JsonProperty("weapon_cat_id")] public int WeaponCatId { get; set; }
        /// <summary>
        /// 武器品质
        /// </summary>
        [JsonProperty("weapon_level")] public int WeaponLevel { get; set; }
    }
}
