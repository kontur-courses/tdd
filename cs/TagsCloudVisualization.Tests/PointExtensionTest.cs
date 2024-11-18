using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.Extension;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class PointExtensionTest
{
    [TestCase(0, 0, 0, 0, 0, 0, Description = "Subtract of zero points")]
    [TestCase(1, 1, 1, 1, 0, 0, Description = "Subtract of positive points")]
    [TestCase(-1, -1, -1, -1, 0, 0, Description = "Subtract of negative points")]
    public void Subtract_ReturnsDifferenceOfPointCoordinates(
        int xPoint1,
        int yPoint1,
        int xPoint2,
        int yPoint2,
        int xResult,
        int yResult)
    {
        var point1 = new Point(xPoint1, yPoint1);
        var point2 = new Point(xPoint2, yPoint2);
        var result = point1.Subtract(point2);
        result.X.Should().Be(xResult);
        result.Y.Should().Be(yResult);
    }

    [Test]
    [TestCaseSource(nameof(PointerSumTestCases))]
    public Point Add_ReturnsSumOfPointCoordinates(
        Point point1,
        Point point2,
        Point correctResult)
    {
        return point1.Add(point2);
    }

    private static IEnumerable<TestCaseData> PointerSumTestCases()
    {
        yield return new TestCaseData(new Point(0, 0), new Point(0, 0), new Point(0, 0))
            .Returns(Point.Empty)
            .SetName("Addition of zero points");
        yield return new TestCaseData(new Point(1, 1), new Point(1, 1), new Point(2, 2))
            .Returns(new Point(2, 2))
            .SetName("Addition of positive points");
        yield return new TestCaseData(new Point(-1, -1), new Point(-1, -1), new Point(-2, -2))
            .Returns(new Point(-2, -2))
            .SetName("Addition of negative points");
    }
}