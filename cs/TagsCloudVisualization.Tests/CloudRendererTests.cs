using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CloudRendererTests
{
    [Test]
    public void Render_RectanglesWithNegativeCoordinates_FitsEntireCloud()
    {
        var padding = 10;
        var renderer = new CloudRenderer(
            padding, 
            Pens.Black, 
            Brushes.LightGray, 
            Color.White);

        Rectangle[] rectangles = [new(-30, -20, 60, 40), new(30, -10, 20, 20)];

        using var image = renderer.Render(rectangles);

        image.Size.Should().Be(new Size(80 + padding * 2, 40 + padding * 2));

        image.GetPixel(20, 20).ToArgb()
            .Should().Be(Color.LightGray.ToArgb());

        image.GetPixel(75, 25).ToArgb()
            .Should().Be(Color.LightGray.ToArgb());

        image.GetPixel(5, 5).ToArgb()
            .Should().Be(Color.White.ToArgb());
    }

    [Test]
    public void CloudRendererConstructor_NegativePadding_ThrowsException()
    {
        var act = () => new CloudRenderer(
            -1,
            Pens.Black,
            Brushes.LightGray,
            Color.White);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void Render_EmptyLayout_ReturnsBlankImage()
    {
        var renderer = new CloudRenderer(
            10,
            Pens.Black,
            Brushes.LightGray,
            Color.White);

        using var image = renderer.Render([]);

        image.Size.Should().Be(new Size(20, 20));

        image.GetPixel(10, 10).ToArgb()
            .Should().Be(Color.White.ToArgb());
    }

    [TestCase(0, 0.05)]
    [TestCase(-1, 0.05)]
    [TestCase(0.8, 0)]
    [TestCase(0.8, -1)]
    [TestCase(double.NaN, 0.05)]
    [TestCase(0.8, double.PositiveInfinity)]
    public void Constructor_InvalidSpiralParameters_ThrowsException(
        double radiusFactor, double angleStep)
    {
        var act = () => new CircularCloudLayouter(Point.Empty, radiusFactor, angleStep);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
