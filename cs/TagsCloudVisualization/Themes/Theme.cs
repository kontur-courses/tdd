using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Themes
{
    public abstract class Theme
    {
        public abstract List<Brush> RectangleBrushes { get; }
        public abstract Brush BackgroundBrush { get; }
        
        internal static SolidBrush GetSolidBrush(string hexColor) => new SolidBrush(ColorTranslator.FromHtml(hexColor));
    }
}