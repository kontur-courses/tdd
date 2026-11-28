using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Layout;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouter_Tests
{
    private Point defaultCenter;
    private CircularCloudLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        defaultCenter = new Point(0, 0);
        layouter = new CircularCloudLayouter(defaultCenter);
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

    [TestCase(0, 0)]
    [TestCase(100, 200)]
    [TestCase(-50, -30)]
    public void PutNextRectangle_FirstRectangle_PlacesCenteredAtCloudCenter(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var customLayouter = new CircularCloudLayouter(center);
        var size = new Size(40, 20);

        var firstRectangle = customLayouter.PutNextRectangle(size);

        var expectedCenter = new PointF(center.X, center.Y);
        firstRectangle.GetCenter().Should().Be(expectedCenter);
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
    public void PutNextRectangle_TwoRectangles_PlacedTightlyWithoutGap()
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
    public void PutNextRectangle_ManyRectangles_DoNotIntersect()
    {
        var sizes = GenerateRandomSizes(count: 150, seed: 42);

        var rectangles = PutAll(layouter, sizes);

        CloudUtils.HasAnyIntersection(rectangles).Should().BeFalse();
    }

    [TestCase(0, 0)]
    [TestCase(250, -150)]
    public void PutNextRectangle_ManyRectangles_KeepsCloudCentered(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var customLayouter = new CircularCloudLayouter(center);
        var sizes = GenerateRandomSizes(count: 100, seed: 42);

        var rectangles = PutAll(customLayouter, sizes);

        var boundingBox = CloudUtils.GetBoundingBox(rectangles);
        var cloudRadius = CloudUtils.GetMaxDistanceFromCenter(rectangles, center);
        var centerOffset = CloudUtils.Distance(boundingBox.GetCenter(), center);
        centerOffset.Should().BeLessOrEqualTo(cloudRadius * 0.15);
    }

    [Test]
    public void PutNextRectangle_ManyVariedRectangles_FormsCircularAspectRatio()
    {
        var sizes = GenerateRandomSizes(count: 150, seed: 42);

        var rectangles = PutAll(layouter, sizes);

        var boundingBox = CloudUtils.GetBoundingBox(rectangles);
        CloudUtils.GetAspectRatio(boundingBox).Should().BeGreaterOrEqualTo(0.8);
    }

    [Test]
    public void PutNextRectangle_ManyVariedRectangles_StaysWithinCompactCircleRadius()
    {
        var sizes = GenerateRandomSizes(count: 150, seed: 42);

        var rectangles = PutAll(layouter, sizes);

        var enclosingRadius = CloudUtils.GetMaxDistanceFromCenter(rectangles, defaultCenter);
        var idealRadius = CloudUtils.GetIdealCircleRadius(rectangles);
        enclosingRadius.Should().BeLessOrEqualTo(idealRadius * 1.35);
    }

    [Test]
    public void PutNextRectangle_ManyVariedRectangles_PacksDenselyWithinEnclosingCircle()
    {
        var sizes = GenerateRandomSizes(count: 150, seed: 42);

        var rectangles = PutAll(layouter, sizes);

        var density = CloudUtils.GetCirclePackingDensity(rectangles, defaultCenter);
        density.Should().BeGreaterOrEqualTo(0.60);
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
            var height = random.Next(10, 40);
            sizes.Add(new Size(width, height));
        }

        return sizes;
    }
}