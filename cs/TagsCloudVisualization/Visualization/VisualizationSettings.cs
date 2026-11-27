using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Visualization;

public record VisualizationSettings(
    Color BackgroundColor,
    Color BorderColor,
    IReadOnlyList<Color> FillColors,
    int Padding)
{
    public static VisualizationSettings Default { get; } = new(
        Color.White,
        Color.Black,
        [Color.SteelBlue, Color.Orange, Color.SeaGreen, Color.IndianRed],
        Padding: 20);
}
