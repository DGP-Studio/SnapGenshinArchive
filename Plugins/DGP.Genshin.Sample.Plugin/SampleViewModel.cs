using CommunityToolkit.Mvvm.ComponentModel;
using Snap.Core.DependencyInjection;
using System.Collections.Generic;
using System.Windows.Media;

namespace DGP.Genshin.Sample.Plugin
{
    /// <summary>
    /// 示例视图模型
    /// </summary>
    [ViewModel(InjectAs.Transient)]
    public class SampleViewModel : ObservableObject
    {
        private IEnumerable<object> icons;

        /// <summary>
        /// 构造一个新的示例视图模型
        /// </summary>
        public SampleViewModel()
        {
            List<object>? list = new();
            ICollection<FontFamily>? families = Fonts.GetFontFamilies(@"C:\Windows\Fonts\segmdl2.ttf");
            foreach (FontFamily family in families)
            {
                ICollection<Typeface>? typefaces = family.GetTypefaces();
                foreach (Typeface typeface in typefaces)
                {
                    typeface.TryGetGlyphTypeface(out GlyphTypeface glyph);
                    IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;

                    foreach (KeyValuePair<int, ushort> kvp in characterMap)
                    {
                        list.Add(new { Glyph = (char)kvp.Key, Data = kvp.Key });
                    }
                }
            }

            icons = list;
        }

        /// <summary>
        /// 图标集合
        /// </summary>
        public IEnumerable<object> Icons
        {
            get => icons;

            set => SetProperty(ref icons, value);
        }
    }
}