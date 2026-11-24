using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class Tests
{

    [Test]
    public void PutNextRectangle_FirstRectangle_PlacesAtCenter()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(60, 40);

        var rectangle = layouter.PutNextRectangle(size);

        rectangle.Should().Be(new Rectangle(-30, -20, 60, 40));
    }

    [Test]
    public void PutNextRectangle_RectanglesDoNotIntersect()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));

        Size[] sizes = [new(60, 40), new(30, 20), new(80, 30), new(20, 20), new(40, 60)];

        var rectangles = sizes
            .Select(layouter.PutNextRectangle)
            .ToArray();

        for (var i = 0; i < rectangles.Length; i++)
            for (var j = i + 1; j < rectangles.Length; j++)
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse();
    }
}
