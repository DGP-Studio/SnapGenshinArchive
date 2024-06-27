namespace DGP.Genshin.Core.Plugins
{
    /// <summary>
    /// 指示该程序集为 Snap Genshin 插件主程序集，使得 Snap Genshin 能够加载你的主程序集
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SnapGenshinPluginAttribute : Attribute
    {
    }
}