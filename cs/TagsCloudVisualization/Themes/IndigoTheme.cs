using System.Collections.Immutable;
using System.Drawing;

namespace TagsCloudVisualization.Themes
{
    public class IndigoTheme : Theme
    {
        public new readonly ImmutableList<Brush> RectangleBrushes = ImmutableList.Create<Brush>(
            GetSolidBrush("#7986CB"),
            GetSolidBrush("#3F51B5"),
            GetSolidBrush("#303F9F"),
            GetSolidBrush("#1A237E"));

        public override Brush BackgroundBrush => GetSolidBrush("#C5CAE9");
    }
}