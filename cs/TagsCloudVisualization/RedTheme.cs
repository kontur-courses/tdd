using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class RedTheme : Theme
    {
        public override List<Brush> RectangleBrushes =>
            new List<Brush>
            {
                GetSolidBrush("#E57373"),
                GetSolidBrush("#F44336"),
                GetSolidBrush("#D32F2F"),
                GetSolidBrush("#B71C1C")
            };

        public override Brush BackgroundBrush => GetSolidBrush("#FFCDD2");
    }
}