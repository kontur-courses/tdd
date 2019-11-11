using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class IndigoTheme : Theme
    {
        public override List<Brush> RectangleBrushes =>
            new List<Brush>
            {
                GetSolidBrush("#7986CB"),
                GetSolidBrush("#3F51B5"),
                GetSolidBrush("#303F9F"),
                GetSolidBrush("#1A237E"),
            };

        public override Brush BackgroundBrush =>  GetSolidBrush("#C5CAE9");
    }
}