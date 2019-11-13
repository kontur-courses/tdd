using System.Collections.Immutable;
using System.Drawing;

namespace TagsCloudVisualization.Themes
{
    public class GreenTheme : Theme
    {
        public new readonly ImmutableList<Brush> RectangleBrushes = ImmutableList.Create<Brush>(
            GetSolidBrush("#81C784"),
            GetSolidBrush("#4CAF50"),
            GetSolidBrush("#388E3C"),
            GetSolidBrush("#1B5E20"));

        public override Brush BackgroundBrush => GetSolidBrush("#C8E6C9");
    }
}