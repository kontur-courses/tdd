using System.Collections.Immutable;
using System.Drawing;

namespace TagsCloudVisualization.Themes
{
    public class RedTheme : Theme
    {
        public new readonly ImmutableList<Brush> RectangleBrushes = ImmutableList.Create<Brush>(
            GetSolidBrush("#E57373"),
            GetSolidBrush("#F44336"),
            GetSolidBrush("#D32F2F"),
            GetSolidBrush("#B71C1C"));

        public override Brush BackgroundBrush => GetSolidBrush("#FFCDD2");
    }
}