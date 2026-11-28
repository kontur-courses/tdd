using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral : IPointGenerator
{
    public double TurnSpacing { get; init; }
    public double PointSpacing { get; init; }
    public double AngleStep { get; init; }

    public ArchimedeanSpiral(double turnSpacing = 5, double pointSpacing = 5, double angleStep = 0.1)
    {
        TurnSpacing = turnSpacing;
        PointSpacing = pointSpacing;
    }

    public IEnumerable<Point> Generate(Point center)
    {
        throw new NotImplementedException();
    }
}