using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private const int Seed = 452;
    private const double MinFillRatio = 0.5;
    private const double MinAspectRatio = 0.75;
    private const double MaxAspectRatio = 1.35;
    private const double MaxCenterOffsetRatio = 0.15;

    private CircularCloudLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        layouter = new();
    }

    [Test]
    public void PutNextRectangle_ShouldReturnValidRectangleSize()
    {
        var size = new Size(10, 10);

        var actual = layouter.PutNextRectangle(size);

        actual.Size.Should().Be(size);
    }

    [TestCase(0, 0, 10, 10)]
    [TestCase(100, -50, 11, 15)]
    [TestCase(-200, 300, 20, 10)]
    public void PutNextRectangle_ShouldReturnRectangleInCenter_WhenRectangleIsSingle(int centerX,
                                                                                     int centerY,
                                                                                     int rectangleWidth,
                                                                                     int rectangleHeight)
    {
        var center = new Point(centerX, centerY);
        layouter = new CircularCloudLayouter(center);
        var size = new Size(rectangleWidth, rectangleHeight);

        var actual = layouter.PutNextRectangle(size);

        var expectedLocation = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
        actual.Location.Should().Be(expectedLocation);
    }

    [TestCaseSource(nameof(GetRandomRectSizes))]
    public void PutNextRectangle_ShouldPackRectanglesTightly(IReadOnlyList<Size> sizes)
    {
        var rectangles = sizes.Select(layouter.PutNextRectangle).ToList();

        var totalArea = GetTotalArea(rectangles);
        var radius = GetMaxCircleRadius(rectangles, layouter.Center);
        var boundingCircleArea = Math.PI * radius * radius;
        var fillRatio = totalArea / boundingCircleArea;

        fillRatio.Should().BeGreaterThanOrEqualTo(MinFillRatio);
    }

    [TestCaseSource(nameof(GetRandomRectSizes))]
    public void PutNextRectangle_ShouldFormCloudWithAspectRatioCloseToOne(IReadOnlyList<Size> sizes)
    {
        var rectangles = sizes.Select(layouter.PutNextRectangle).ToList();

        var boundingBox = GetCloudBox(rectangles);
        var aspectRatio = (double)boundingBox.Width / boundingBox.Height;

        aspectRatio.Should().BeInRange(MinAspectRatio, MaxAspectRatio);
    }

    [TestCase(0, 10, TestName = "width is zero")]
    [TestCase(10, 0, TestName = "height is zero")]
    [TestCase(-5, 10, TestName = "width is negative")]
    [TestCase(10, -5, TestName = "height is negative")]
    public void PutNextRectangle_ShouldThrowArgumentException_WhenSizeIsNotPositive(int width, int height)
    {
        var size = new Size(width, height);

        var action = () => layouter.PutNextRectangle(size);

        action.Should().Throw<ArgumentException>();
    }

    [TestCaseSource(nameof(GetRandomRectSizes))]
    public void PutNextRectangle_ShouldReturnNotIntersectedRectangles(IReadOnlyList<Size> sizes)
    {
        var rectangles = sizes.Select(layouter.PutNextRectangle).ToList();

        HasAnyIntersections(rectangles).Should().BeFalse();
    }

    [TestCaseSource(nameof(GetRandomRectSizes))]
    public void PutNextRectangle_ShouldCenterCloudAroundGivenPoint(IReadOnlyList<Size> sizes)
    {
        var center = new Point(300, -200);
        layouter = new CircularCloudLayouter(center);

        var rectangles = sizes.Select(layouter.PutNextRectangle).ToList();

        var boundingBox = GetCloudBox(rectangles);
        var actualCenter = GetCenter(boundingBox);
        var maxAllowedOffset = Math.Min(boundingBox.Width, boundingBox.Height) * MaxCenterOffsetRatio;

        GetDistanceBetween(actualCenter, center).Should().BeLessThanOrEqualTo(maxAllowedOffset);
    }

    [Test]
    public void PutNextRectangle_ShouldPlaceSecondRectangleAdjacentToFirst()
    {
        var first = layouter.PutNextRectangle(new Size(20, 40));
        var second = layouter.PutNextRectangle(new Size(30, 10));

        var gapX = Math.Max(0, Math.Max(first.Left - second.Right, second.Left - first.Right));
        var gapY = Math.Max(0, Math.Max(first.Top - second.Bottom, second.Top - first.Bottom));

        Math.Max(gapX, gapY).Should().Be(0);
    }

    private static IEnumerable<TestCaseData> GetRandomRectSizes()
    {
        var rnd = new Random(Seed);
        var rectCount = rnd.Next(50, 100);
        var sizes = new List<Size>(rectCount);

        for (var i = 0; i < rectCount; i++)
            sizes.Add(new(rnd.Next(5, 100), rnd.Next(5, 100)));

        yield return new TestCaseData(sizes).SetName("RandomRectSizesFor_{m}");
    }

    private static bool HasAnyIntersections(List<Rectangle> rectangles) =>
        Enumerable.Range(0, rectangles.Count)
                  .Any(i => rectangles.Skip(i + 1)
                  .Any(other => rectangles[i].IntersectsWith(other)));

    private static Rectangle GetCloudBox(IReadOnlyCollection<Rectangle> rectangles) =>
        Rectangle.FromLTRB(rectangles.Min(r => r.Left),
                           rectangles.Min(r => r.Top),
                           rectangles.Max(r => r.Right),
                           rectangles.Max(r => r.Bottom));

    private static double GetMaxCircleRadius(IEnumerable<Rectangle> rectangles, Point center) =>
        rectangles.SelectMany(GetCorners)
                  .Max(corner => GetDistanceBetween(corner, center));

    private static Point[] GetCorners(Rectangle rect) =>
    [
        new(rect.Left, rect.Top),
        new(rect.Right, rect.Top),
        new(rect.Left, rect.Bottom),
        new(rect.Right, rect.Bottom)
    ];

    private static double GetTotalArea(IEnumerable<Rectangle> rectangles) =>
        rectangles.Sum(r => (double)r.Width * r.Height);

    private static Point GetCenter(Rectangle rect) =>
        new(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

    private static double GetDistanceBetween(Point a, Point b)
    {
        var dx = (double)a.X - b.X;
        var dy = (double)a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

}
