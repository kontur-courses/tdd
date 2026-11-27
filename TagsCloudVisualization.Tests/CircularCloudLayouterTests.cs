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
    public void PutNextRectangle_ManyRectanglesOfDifferentSizes_DoNotIntersect()
    {
        const int rectangleCount = 30;
        var random = new Random(42);
        var layouter = new CircularCloudLayouter(new Point(100, 100));
        var sizes = Enumerable.Range(0, rectangleCount)
            .Select(_ => new Size(random.Next(5, 31), random.Next(5, 31)))
            .ToArray();

        var rectangles = sizes.Select(layouter.PutNextRectangle).ToArray();

        for (var i = 0; i < rectangles.Length; i++)
        {
            for (var j = i + 1; j < rectangles.Length; j++)
            {
                Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False,
                    $"Rectangle {i} {rectangles[i]} intersects rectangle {j} {rectangles[j]}.");
            }
        }
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

    [Test]
    public void PutNextRectangle_ManyEqualSquares_CloudStaysNearRequestedCenter()
    {
        var center = new Point(100, 200);
        var squareSize = new Size(10, 10);
        const int squareCount = 100;
        var maxDistance = squareSize.Width;
        var layouter = new CircularCloudLayouter(center);

        var rectangles = Enumerable.Range(0, squareCount)
            .Select(_ => layouter.PutNextRectangle(squareSize))
            .ToArray();

        var bounds = rectangles.Aggregate(Rectangle.Union);
        var offsetX = bounds.Left + bounds.Width / 2.0 - center.X;
        var offsetY = bounds.Top + bounds.Height / 2.0 - center.Y;
        var distance = Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
        Assert.That(distance, Is.LessThanOrEqualTo(maxDistance));
    }

    [Test]
    public void PutNextRectangle_ManyEqualSquares_CloudFillsAtLeastHalfOfItsBounds()
    {
        const int squareCount = 100;
        var squareSize = new Size(10, 10);
        var layouter = new CircularCloudLayouter(new Point(100, 100));

        var rectangles = Enumerable.Range(0, squareCount)
            .Select(_ => layouter.PutNextRectangle(squareSize))
            .ToArray();

        var bounds = rectangles.Aggregate(Rectangle.Union);
        var occupiedArea = rectangles.Sum(rectangle => rectangle.Width * rectangle.Height);
        var boundsArea = (double)bounds.Width * bounds.Height;
        Assert.That(occupiedArea / boundsArea, Is.GreaterThanOrEqualTo(0.5));
    }
}
