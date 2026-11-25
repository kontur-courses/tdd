using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouterTests
{
    [Test]
    public void PutNextRectangle_FirstRectangleIsCenteredAtCloudCenter()
    {
        var center = new Point(100, 100);
        var rectangleSize = new Size(40, 20);
        var layouter = new CircularCloudLayouter(center);

        var rectangle = layouter.PutNextRectangle(rectangleSize);

        Assert.That(rectangle, Is.EqualTo(new Rectangle(80, 90, 40, 20)));
    }

    [Test]
    public void PutNextRectangle_SecondRectangleDoesNotIntersectFirst()
    {
        var layouter = new CircularCloudLayouter(new Point(100, 100));

        var first = layouter.PutNextRectangle(new Size(40, 20));
        var second = layouter.PutNextRectangle(new Size(30, 10));

        Assert.That(second.IntersectsWith(first), Is.False);
    }

    [Test]
    public void PutNextRectangle_DoesNotIntersectAnyPreviouslyPlacedRectangle()
    {
        var layouter = new CircularCloudLayouter(new Point(100, 100));

        var sizes = new[]
        {
            new Size(40, 20),
            new Size(30, 10),
            new Size(25, 30),
            new Size(15, 15),
            new Size(50, 10)
        };

        var rectangles = new List<Rectangle>();

        foreach (var size in sizes)
            rectangles.Add(layouter.PutNextRectangle(size));

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                Assert.That(
                    rectangles[i].IntersectsWith(rectangles[j]),
                    Is.False,
                    $"Rectangles {i} and {j} intersect.");
            }
        }
    }
}