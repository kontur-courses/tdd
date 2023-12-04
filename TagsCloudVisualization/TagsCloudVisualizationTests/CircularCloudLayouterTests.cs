using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    [Test]
    public void Constructor_Success_OnPointArgument()
    {
        var a = () => new CircularCloudLayouter(Point.Empty);

        a.Should().NotThrow();
    }
}