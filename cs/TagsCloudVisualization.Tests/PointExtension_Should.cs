using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class PointExtension_Should
{
    [TestCase(0, 0, 0, 0, ExpectedResult = 0)]
    [TestCase(0, 0, 3, 4, ExpectedResult = 5)]
    [TestCase(0, 0, -3, -4, ExpectedResult = 5)]
    [TestCase(-1, -1, -4, -5, ExpectedResult = 5)]
    public double GetDistance_ShouldCalculate_DistanceCorrectly(int x1, int y1, int x2, int y2)
    {
        var point1 = new Point(x1, y1);
        var point2 = new Point(x2, y2);
        return point1.GetDistance(point2);
    }
}