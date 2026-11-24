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
}
