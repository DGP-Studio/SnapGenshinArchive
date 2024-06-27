using DGP.Genshin.Core;
using DGP.Genshin.Core.LifeCycle;
using DGP.Genshin.Core.Plugins;
using System;
using System.Windows.Media;

[assembly: SnapGenshinPlugin]

namespace DGP.Genshin.Skin.Pink;

/// <summary>
/// 粉色皮肤插件
/// </summary>
public class PinkSkinPlugin : IPlugin, IAppStartUp
{
    /// <inheritdoc/>
    public bool IsEnabled { get => true; }

    /// <inheritdoc/>
    public string Name { get => "Shame 粉"; }

    /// <inheritdoc/>
    public string Description { get => "用 ❤️ 制作，可以让你的 Snap Genshin 拥有全新的外观。"; }

    /// <inheritdoc/>
    public string Author { get => "最爱 Shame 的 希尔"; }

    /// <inheritdoc/>
    public Version Version { get => new(1, 0, 0, 1); }

    /// <inheritdoc/>
    public void Happen(IContainer container)
    {
        Color pinkColorLight2 = Color.FromArgb(255, 255, 199, 190);
        Color pinkColorLight1 = Color.FromArgb(255, 255, 194, 190);
        Color pinkColor = Color.FromArgb(255, 230, 172, 172);

        // WPFUI
        App.Current.Resources["TextFillColorPrimary"] = pinkColor;
        App.Current.Resources["TextFillColorSecondary"] = pinkColorLight1;
        App.Current.Resources["TextFillColorTertiary"] = pinkColorLight2;

        App.Current.Resources.MergedDictionaries.Add(new()
        {
            Source = new("pack://application:,,,/DGP.Genshin.Skin.Pink;component/Dark.xaml"),
        });
    }
}