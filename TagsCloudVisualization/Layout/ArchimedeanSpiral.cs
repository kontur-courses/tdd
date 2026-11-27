using System.Drawing;

namespace TagsCloudVisualization;

/// <summary>
/// Перебирает целочисленные смещения по архимедовой спирали, начиная с центра.
/// </summary>
public sealed class ArchimedeanSpiral : ICandidatePositionSource
{
    private const double AngleStep = 0.1;
    private const double RadiusGrowth = 1.0;

    /// <inheritdoc />
    public IEnumerable<Point> GetOffsets()
    {
        for (var angle = 0.0; ; angle += AngleStep)
        {
            var radius = RadiusGrowth * angle;
            yield return new Point(
                (int)Math.Round(radius * Math.Cos(angle)),
                (int)Math.Round(radius * Math.Sin(angle)));
        }
    }
}
