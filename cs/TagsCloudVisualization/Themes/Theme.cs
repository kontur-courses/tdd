using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace TagsCloudVisualization.Themes
{
    public abstract class Theme
    {
        public readonly ImmutableList<Brush> RectangleBrushes = ImmutableList.Create(Brushes.Black);
        public abstract Brush BackgroundBrush { get; }

        internal static SolidBrush GetSolidBrush(string hexColor) => new SolidBrush(ColorTranslator.FromHtml(hexColor));
    }
}