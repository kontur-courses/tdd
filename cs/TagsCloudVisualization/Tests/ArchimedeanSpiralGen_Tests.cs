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

        var points = spiral.GetPoints().Take(100).ToList();

        for (var i = 1; i < points.Count; i++)
            points[i].Should().NotBe(points[i - 1]);
    }

    [Test]
    public void GetPoints_MultiplePoints_MovesFromCenter()
    {
        var center = new Point(0, 0);
        var spiral = new ArchimedeanSpiralGen(center);

        var points = spiral.GetPoints().Take(500).ToList();

        var earlyDistance = CloudUtils.Distance(points[10], center);
        var lateDistance = CloudUtils.Distance(points[^1], center);
        lateDistance.Should().BeGreaterThan(earlyDistance);
    }
}