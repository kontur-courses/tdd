using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators;

public interface IPointsGenerator
{
    public IEnumerable<Point> GeneratePoints(Point startPoint);
}