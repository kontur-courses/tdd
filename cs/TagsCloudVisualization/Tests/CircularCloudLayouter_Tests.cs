using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Layout;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouter_Tests
{
    [SetUp]
    public void SetUp()
    {
        defaultCenter = new Point(0, 0);
        layouter = new CircularCloudLayouter(defaultCenter);
    }

    private Point defaultCenter;
    private CircularCloudLayouter layouter = null!;

    [TestCase(0, 0)]
    [TestCase(120, -80)]
    public void Constructor_ValidCenter_InitializesCenterAndEmptyRectangles(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);

        layouter = new CircularCloudLayouter(center);

        layouter.Center.Should().Be(center);
        layouter.Rectangles.Should().BeEmpty();
    }

    [TestCase(0, 10)]
    [TestCase(10, 0)]
    [TestCase(-5, 10)]
    [TestCase(10, -5)]
    [TestCase(0, 0)]
    public void PutNextRectangle_NonPositiveSize_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        var invalidSize = new Size(width, height);

        var act = () => layouter.PutNextRectangle(invalidSize);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(10, 10)]
    [TestCase(50, 20)]
    [TestCase(1, 100)]
    public void PutNextRectangle_ValidSize_ReturnsRectangleWithRequestedSize(int width, int height)
    {
        var requestedSize = new Size(width, height);

        var rectangle = layouter.PutNextRectangle(requestedSize);

        rectangle.Size.Should().Be(requestedSize);
    }

    [Test]
    public void PutNextRectangle_MultipleCalls_StoresPlacedRectanglesInOrder()
    {
        var sizes = new[] { new Size(40, 20), new Size(30, 15), new Size(25, 25) };

        var returned = PutAll(layouter, sizes);

        layouter.Rectangles.Should().Equal(returned);
    }

    [TestCase(0, 0, 40, 20)]
    [TestCase(100, 200, 40, 20)]
    [TestCase(-50, -30, 41, 21)]
    public void PutNextRectangle_FirstRectangle_PlacesCenteredAtCloudCenter(
        int centerX,
        int centerY,
        int width,
        int height)
    {
        var center = new Point(centerX, centerY);
        layouter = new CircularCloudLayouter(center);
        var size = new Size(width, height);

        var firstRectangle = layouter.PutNextRectangle(size);

        CloudUtils.Distance(firstRectangle.GetCenter(), center).Should().BeLessThan(1.0);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_DoNotIntersect()
    {
        var size = new Size(40, 20);

        var first = layouter.PutNextRectangle(size);
        var second = layouter.PutNextRectangle(size);

        first.IntersectsWith(second).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_TouchWithoutGapOrOverlap()
    {
        var size = new Size(40, 20);

        var first = layouter.PutNextRectangle(size);
        var second = layouter.PutNextRectangle(size);

        CloudUtils.GetGapBetween(first, second).Should().Be(0);
    }

    [Test]
    public void PutNextRectangle_ManyUniformRectangles_DoNotIntersect()
    {
        var sizes = Enumerable.Repeat(new Size(30, 15), 100).ToList();

        var rectangles = PutAll(layouter, sizes);

        CloudUtils.HasAnyIntersection(rectangles).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ManyVariedRectangles_DoNotIntersect()
    {
        var sizes = GenerateRandomSizes(150, 42);

        var rectangles = PutAll(layouter, sizes);

        CloudUtils.HasAnyIntersection(rectangles).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_DescendingTagSizes_PacksDenselyWithoutIntersections()
    {
        var sizes = GenerateRandomSizes(150, 7)
            .OrderByDescending(s => s.Width * s.Height)
            .ToList();

        var rectangles = PutAll(layouter, sizes);

        CloudUtils.HasAnyIntersection(rectangles).Should().BeFalse();
        CloudUtils.GetCirclePackingDensity(rectangles, defaultCenter).Should().BeGreaterThan(2.0 / Math.PI);
    }

    [TestCase(0, 0)]
    [TestCase(250, -150)]
    public void PutNextRectangle_ManyRectangles_KeepsCloudCentered(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        layouter = new CircularCloudLayouter(center);
        var sizes = GenerateRandomSizes(150, 42);

        var rectangles = PutAll(layouter, sizes);

        var boundingBox = CloudUtils.GetBoundingBox(rectangles);
        var cloudRadius = CloudUtils.GetMaxDistanceFromCenter(rectangles, center);
        var centerOffset = CloudUtils.Distance(boundingBox.GetCenter(), center);
        centerOffset.Should().BeLessOrEqualTo(cloudRadius * 0.15);
    }

    [Test]
    public void PutNextRectangle_ManyVariedRectangles_FormsCircularAspectRatio()
    {
        var sizes = GenerateRandomSizes(150, 42);

        var rectangles = PutAll(layouter, sizes);

        var boundingBox = CloudUtils.GetBoundingBox(rectangles);
        CloudUtils.GetAspectRatio(boundingBox).Should().BeGreaterOrEqualTo(0.85);
    }

    [Test]
    public void PutNextRectangle_ManyVariedRectangles_PacksDenselyIntoCircularShape()
    {
        var sizes = GenerateRandomSizes(150, 42);

        var rectangles = PutAll(layouter, sizes);

        // Любой прямоугольник, вписанный в окружность, имеет плотность не выше 2 / π(≈ 0.637)
        // Превышение этого предела доказывает одновременно
        // высокую плотность упаковки и скруглённую (не квадратную) форму облака.
        var maxPossibleRectangleInCircleDensity = 2.0 / Math.PI;
        var density = CloudUtils.GetCirclePackingDensity(rectangles, defaultCenter);
        density.Should().BeGreaterThan(maxPossibleRectangleInCircleDensity);
    }

    private static List<Rectangle> PutAll(CircularCloudLayouter targetLayouter, IEnumerable<Size> sizes)
    {
        return sizes.Select(targetLayouter.PutNextRectangle).ToList();
    }

    private static List<Size> GenerateRandomSizes(int count, int seed)
    {
        var random = new Random(seed);
        var sizes = new List<Size>(count);

        for (var i = 0; i < count; i++)
        {
            var width = random.Next(15, 80);
            var height = random.Next(10, 50);
            sizes.Add(new Size(width, height));
        }

        return sizes;
    }
}