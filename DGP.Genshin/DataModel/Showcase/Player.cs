namespace DGP.Genshin.DataModel.Showcase;

public class Player
{
    public string NickName { get; set; } = default!;

    public int Level { get; set; }

    public string Signature { get; set; } = default!;

    public int WorldLevel {get; set; }

    public int AchievementNumber { get; set; }

    public string SpiralAbyss { get; set; } = default!;

    public string ProfilePicture { get; set; } = default!;
}