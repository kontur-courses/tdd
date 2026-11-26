using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    [Test]
    public void PutNextRectangle_FirstRectangle_IsCenteredAtCloudCenter()
    {
        var center = new Point(100, 200);
        var rectangleSize = new Size(20, 10);
        var layouter = new CircularCloudLayouter(center);

        var rectangle = layouter.PutNextRectangle(rectangleSize);

        Assert.That(rectangle, Is.EqualTo(new Rectangle(90, 195, 20, 10)));
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_DoNotIntersect()
    {
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var rectangleSize = new Size(20, 10);
        var firstRectangle = layouter.PutNextRectangle(rectangleSize);

        var secondRectangle = layouter.PutNextRectangle(rectangleSize);

        Assert.That(secondRectangle.IntersectsWith(firstRectangle), Is.False);
    }

    [Test]
    public void PutNextRectangle_ManyEqualSquares_CloudHasComparableWidthAndHeight()
    {
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var squareSize = new Size(10, 10);
        const int squareCount = 100;

        var rectangles = Enumerable.Range(0, squareCount)
            .Select(_ => layouter.PutNextRectangle(squareSize))
            .ToArray();

        var bounds = rectangles.Aggregate(Rectangle.Union);
        var aspectRatio = bounds.Width / (double)bounds.Height;
        Assert.That(aspectRatio, Is.InRange(0.5, 2.0));
    }
}
