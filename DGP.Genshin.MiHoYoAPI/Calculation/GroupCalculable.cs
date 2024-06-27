using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Calculation
{
    public abstract class GroupCalculable : Calculable
    {
        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        public override PromotionDelta ToPromotionDelta()
        {
            return new()
            {
                LevelCurrent = LevelCurrent,
                LevelTarget = LevelTarget,
                Id = GroupId,
            };
        }
    }
}
