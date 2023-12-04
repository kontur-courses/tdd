using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    [Test]
    public void Constructor_Success_OnPointArgument()
    {
        var a = () => new CircularCloudLayouter(Point.Empty);

        a.Should().NotThrow();
    }
    
    [Test]
    public void PutNextRectangle_ReturnsRectangle_WithProvidedSize()
    {
        var center = Point.Empty;
        var layouter = new CircularCloudLayouter(center);
        var rectSize = new Size(2, 2);

        layouter.PutNextRectangle(rectSize).Size.Should().Be(rectSize);
    }
}