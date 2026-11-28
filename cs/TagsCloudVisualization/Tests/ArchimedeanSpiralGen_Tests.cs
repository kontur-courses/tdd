using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Layout;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class ArchimedeanSpiralGen_Tests
{
    [TestCase(0, 0)]
    [TestCase(100, -50)]
    public void GetPoints_FirstPoint_ReturnsCenter(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var spiral = new ArchimedeanSpiralGen(center);

        var firstPoint = spiral.GetPoints().First();

        firstPoint.Should().Be(center);
    }

    [Test]
    public void GetPoints_ConsecutivePoints_DoesNotRepeat()
    {
        var spiral = new ArchimedeanSpiralGen(new Point(0, 0));

        var points = spiral.GetPoints().Take(200).ToList();

        for (var i = 1; i < points.Count; i++)
            points[i].Should().NotBe(points[i - 1]);
    }

    [Test]
    public void GetPoints_MultipleTurns_VisitsAllQuadrantsAroundCenter()
    {
        var center = new Point(50, -30);
        var spiral = new ArchimedeanSpiralGen(center);

        var points = spiral.GetPoints().Take(500).ToList();

        points.Should().Contain(p => p.X > center.X && p.Y > center.Y);
        points.Should().Contain(p => p.X < center.X && p.Y > center.Y);
        points.Should().Contain(p => p.X < center.X && p.Y < center.Y);
        points.Should().Contain(p => p.X > center.X && p.Y < center.Y);
    }

    [Test]
    public void GetPoints_MultiplePoints_ExpandsFromCenter()
    {
        var center = new Point(0, 0);
        var spiral = new ArchimedeanSpiralGen(center);

        var points = spiral.GetPoints().Take(1000).ToList();

        var nearDistance = CloudUtils.Distance(points[10], center);
        var midDistance = CloudUtils.Distance(points[200], center);
        var farDistance = CloudUtils.Distance(points[^1], center);
        midDistance.Should().BeGreaterThan(nearDistance);
        farDistance.Should().BeGreaterThan(midDistance);
    }
}
