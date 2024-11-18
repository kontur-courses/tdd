using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class SpiralTest
{
    [TestCase(0, 0, 1)]
    [TestCase(2, 10, 2)]
    [TestCase(-1, -1, 2)]
    public void Constructor_WithCorrectParameters_NotThrow(int x, int y, double step)
    {
        var action = () => new Spiral(new(x, y), step);
        action.Should().NotThrow<ArgumentException>();
    }

    [Test]
    public void GetNextPoint_WithCorrectParameters_ReturnsCorrectPoint()
    {
        var spiral = new Spiral(new(0, 0), 1);
        spiral.GetNextPoint().Should().Be(new Point(0, 0));
        spiral.GetNextPoint().Should().Be(new Point((int)(Math.PI / 40 * Math.Cos(Math.PI / 40)),
            (int)(Math.PI / 40 * Math.Sin(Math.PI / 40))));
    }

    [Test]
    public void GetNextPoint_WithCorrectParameters_ReturnsCentralPoint()
    {
        var spiral = new Spiral(new(1, 2), 1);
        spiral.GetNextPoint().Should().Be(new Point(1, 2));
    }
}