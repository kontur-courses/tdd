using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private ICloudLayouter defaultLayouter = null!;

    [SetUp]
    public void SetUp()
    {
        defaultLayouter = new CircularCloudLayouter(new Point(100, 100));
    }

    [TestCase(0, 10)]
    [TestCase(10, 0)]
    [TestCase(-1, 10)]
    [TestCase(10, -1)]
    public void PutNextRectangle_NonPositiveDimension_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        var rectangleSize = new Size(width, height);

        TestDelegate placeRectangle = () => defaultLayouter.PutNextRectangle(rectangleSize);

        Assert.That(placeRectangle, Throws.TypeOf<ArgumentOutOfRangeException>());
    }

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
        var rectangleSize = new Size(20, 10);
        var firstRectangle = defaultLayouter.PutNextRectangle(rectangleSize);

        var secondRectangle = defaultLayouter.PutNextRectangle(rectangleSize);

        Assert.That(secondRectangle.IntersectsWith(firstRectangle), Is.False);
    }

    [Test]
    public void PutNextRectangle_WithCustomDependencies_StoresFirstFreeCandidate()
    {
        var center = new Point(100, 100);
        var positionSource = new FixedPositionSource(Point.Empty, new Point(40, 0));
        var placedRectangles = new PlacedRectangleList();
        ICloudLayouter layouter = new CircularCloudLayouter(center, positionSource, placedRectangles);
        var rectangleSize = new Size(10, 10);
        layouter.PutNextRectangle(rectangleSize);

        var rectangle = layouter.PutNextRectangle(rectangleSize);

        Assert.That(rectangle, Is.EqualTo(new Rectangle(135, 95, 10, 10)));
        Assert.That(placedRectangles.Intersects(rectangle), Is.True);
    }

    [Test]
    public void PutNextRectangle_WhenAllCandidatesAreOccupied_ThrowsInvalidOperationException()
    {
        var positionSource = new FixedPositionSource(Point.Empty);
        var layouter = new CircularCloudLayouter(Point.Empty, positionSource, new PlacedRectangleList());
        var rectangleSize = new Size(10, 10);
        layouter.PutNextRectangle(rectangleSize);

        TestDelegate placeRectangle = () => layouter.PutNextRectangle(rectangleSize);

        Assert.That(placeRectangle, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void PutNextRectangle_ManyRectanglesOfDifferentSizes_DoNotIntersect()
    {
        const int rectangleCount = 30;
        var random = new Random(42);
        var sizes = Enumerable.Range(0, rectangleCount)
            .Select(_ => new Size(random.Next(5, 31), random.Next(5, 31)))
            .ToArray();

        var rectangles = sizes.Select(defaultLayouter.PutNextRectangle).ToArray();

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
        var squareSize = new Size(10, 10);
        const int squareCount = 100;

        var rectangles = Enumerable.Range(0, squareCount)
            .Select(_ => defaultLayouter.PutNextRectangle(squareSize))
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

        var rectangles = Enumerable.Range(0, squareCount)
            .Select(_ => defaultLayouter.PutNextRectangle(squareSize))
            .ToArray();

        var bounds = rectangles.Aggregate(Rectangle.Union);
        var occupiedArea = rectangles.Sum(rectangle => rectangle.Width * rectangle.Height);
        var boundsArea = (double)bounds.Width * bounds.Height;
        Assert.That(occupiedArea / boundsArea, Is.GreaterThanOrEqualTo(0.5));
    }

    private sealed class FixedPositionSource(params Point[] offsets) : ICandidatePositionSource
    {
        public IEnumerable<Point> GetOffsets() => offsets;
    }
}
