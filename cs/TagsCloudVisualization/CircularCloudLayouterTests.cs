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
                Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False, $"Rectangles {i} and {j} intersect.");
            }
        }
    }

    [Test]
    public void PutNextRectangle_SquareRectanglesProduceApproximatelySquareBounds()
    {
        const int rectangleCount = 50;
        var rectangleSize = new Size(20, 20);
        var layouter = new CircularCloudLayouter(new Point(500, 500));

        var rectangles = PlaceRectangles(
            layouter,
            rectangleSize,
            rectangleCount);

        var bounds = GetBoundingRectangle(rectangles);
        var aspectRatio = (double)bounds.Width / bounds.Height;

        Assert.That(aspectRatio, Is.InRange(0.85, 1.18));
    }

    [Test]
    public void PutNextRectangle_PacksRectanglesIntoCompactArea()
    {
        const int rectangleCount = 50;
        var rectangleSize = new Size(20, 20);
        var layouter = new CircularCloudLayouter(new Point(500, 500));

        var rectangles = PlaceRectangles(
            layouter,
            rectangleSize,
            rectangleCount);

        var bounds = GetBoundingRectangle(rectangles);
        var occupiedArea = rectangles.Sum(
            rectangle => (long)rectangle.Width * rectangle.Height);

        var boundingArea = (long)bounds.Width * bounds.Height;
        var density = (double)occupiedArea / boundingArea;

        Assert.That(density, Is.GreaterThanOrEqualTo(0.55));
    }

    [Test]
    public void PutNextRectangle_SecondRectangleCannotMoveCloserWithoutIntersectingFirst()
    {
        var center = new Point(100, 100);
        var layouter = new CircularCloudLayouter(center);

        var first = layouter.PutNextRectangle(new Size(40, 20));
        var second = layouter.PutNextRectangle(new Size(30, 10));

        var currentDistance = GetDistanceSquared(second, center);
        var hasCloserPosition = false;

        for (var offsetX = -1; offsetX <= 1; offsetX++)
        {
            for (var offsetY = -1; offsetY <= 1; offsetY++)
            {
                if (offsetX == 0 && offsetY == 0)
                    continue;

                var candidate = new Rectangle(
                    second.X + offsetX,
                    second.Y + offsetY,
                    second.Width,
                    second.Height);

                if (GetDistanceSquared(candidate, center) >= currentDistance)
                    continue;

                hasCloserPosition = true;

                Assert.That(candidate.IntersectsWith(first),Is.True, "A closer position should intersect the first rectangle.");
            }
        }

        Assert.That(hasCloserPosition, Is.True);
    }

    [TestCase(0, 10)]
    [TestCase(10, 0)]
    [TestCase(-1, 10)]
    [TestCase(10, -1)]
    public void PutNextRectangle_ThrowsWhenRectangleSizeIsInvalid(int width, int height)
    {
        var layouter = new CircularCloudLayouter(new Point(100, 100));

        Assert.Throws<ArgumentOutOfRangeException>(() => layouter.PutNextRectangle(new Size(width, height)));
    }

    [Test]
    public void PutNextRectangle_InvalidSizeDoesNotChangeLayout()
    {
        var center = new Point(100, 100);
        var layouter = new CircularCloudLayouter(center);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => layouter.PutNextRectangle(new Size(0, 20)));

        var rectangle = layouter.PutNextRectangle(new Size(40, 20));

        Assert.That(rectangle, Is.EqualTo(new Rectangle(80, 90, 40, 20)));
    }

    private static Rectangle[] PlaceRectangles(CircularCloudLayouter layouter, Size size, int count)
    {
        var rectangles = new Rectangle[count];

        for (var i = 0; i < count; i++)
            rectangles[i] = layouter.PutNextRectangle(size);

        return rectangles;
    }

    private static Rectangle GetBoundingRectangle(IReadOnlyList<Rectangle> rectangles)
    {
        var bounds = rectangles[0];

        for (var i = 1; i < rectangles.Count; i++)
            bounds = Rectangle.Union(bounds, rectangles[i]);

        return bounds;
    }

    private static double GetDistanceSquared(Rectangle rectangle, Point center)
    {
        var deltaX = rectangle.X + rectangle.Width / 2.0 - center.X;
        var deltaY = rectangle.Y + rectangle.Height / 2.0 - center.Y;

        return deltaX * deltaX + deltaY * deltaY;
    }
}