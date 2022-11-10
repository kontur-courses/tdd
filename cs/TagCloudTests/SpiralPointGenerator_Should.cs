using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests;

public class SpiralPointGenerator_Should
{
    [Test]
    public void Generate_FirstPoint_AtCentre()
    {
        var center = new Point(0, 0);
        var generator = new SpiralPointGenerator(1, Math.PI / 3);

        var firstPoint = generator.Generate(center).First();

        firstPoint.Should().BeEquivalentTo(center);
    }

    [Test]
    public void Generate_GeneratedLine_IntersectsCoordinateAxesAtCorrectPoints()
    {
        var generator = new SpiralPointGenerator(1, 45 / (180 / Math.PI));

        var points = generator.Generate(new Point(0, 0)).Take(600).ToArray();
        var intersectXAxe = points
            .Skip(1)
            .Select((point, i) => (point, r: i + 2))
            .Where(t => t.point.Y == 0);
        var intersectYAxe = points
            .Skip(2)
            .Select((point, i) => (point, r: i + 3))
            .Where(t => t.point.X == 0);

        intersectXAxe.Should().AllSatisfy(t => Math.Abs(t.point.X).Should().Be(t.r));
        intersectYAxe.Should().AllSatisfy(t => Math.Abs(t.point.Y).Should().Be(t.r));
    }
}