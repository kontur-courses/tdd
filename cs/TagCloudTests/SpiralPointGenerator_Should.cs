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
        var generator = new SpiralPointGenerator(1, Math.PI / 3);

        var points = generator.Generate(new Point(0, 0)).Take(600).ToArray();
        var intersectXAxe = points.Where(p => p.Y == 0).Select((p, i) => (x: Math.Abs(p.X), r: i * 3));
        var intersectYAxe = points.Where(p => p.X == 0).Select((p, i) => (y: Math.Abs(p.Y), r: i));

        intersectXAxe.Should().AllSatisfy(t => t.x.Should().Be(t.r));
        intersectYAxe.Should().AllSatisfy(t => t.y.Should().Be(t.r));
    }
}