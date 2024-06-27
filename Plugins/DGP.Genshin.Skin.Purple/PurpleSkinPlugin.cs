using DGP.Genshin.Core;
using DGP.Genshin.Core.LifeCycle;
using DGP.Genshin.Core.Plugins;
using System;
using System.Windows.Media;

[assembly: SnapGenshinPlugin]

namespace DGP.Genshin.Skin.Purple;

/// <summary>
/// 粉色皮肤插件
/// </summary>
public class PurpleSkinPlugin : IPlugin, IAppStartUp
{
    /// <inheritdoc/>
    public bool IsEnabled { get => true; }

    /// <inheritdoc/>
    public string Name { get => "Shame 紫"; }

    /// <inheritdoc/>
    public string Description { get => "用 ❤️ 制作，可以让你的 Snap Genshin 拥有全新的外观。"; }

    /// <inheritdoc/>
    public string Author { get => "最爱 Shame 的 希尔"; }

    /// <inheritdoc/>
    public Version Version { get => new(1, 0, 0, 1); }

    /// <inheritdoc/>
    public void Happen(IContainer container)
    {
        Color colorLight2 = Color.FromArgb(255, 224, 146, 255);
        Color colorLight1 = Color.FromArgb(255, 217, 146, 255);
        Color color = Color.FromArgb(255, 181, 126, 220);

        // WPFUI
        App.Current.Resources["TextFillColorPrimary"] = color;
        App.Current.Resources["TextFillColorSecondary"] = colorLight1;
        App.Current.Resources["TextFillColorTertiary"] = colorLight2;

        App.Current.Resources.MergedDictionaries.Add(new()
        {
            Source = new("pack://application:,,,/DGP.Genshin.Skin.Purple;component/Dark.xaml"),
        });
    }
}