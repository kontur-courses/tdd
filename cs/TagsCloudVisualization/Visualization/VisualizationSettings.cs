using System.Drawing;

namespace TagsCloudVisualization.Visualization;

public class VisualizationSettings
{
    public int Padding { get; init; } = 40;
    public Color BackgroundColor { get; init; } = Color.FromArgb(24, 24, 32);
    public Color BorderColor { get; init; } = Color.FromArgb(220, 230, 245);
    public float BorderWidth { get; init; } = 1.5f;
    public bool DrawCenterMarker { get; init; } = true;
    public bool DrawEnclosingCircle { get; init; } = false;
}