using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Layout;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter? layouter;

    [SetUp]
    public void SetUpLayouter()
    {
        layouter = CreateDefaultLayouter(Point.Empty);
    }

    [TearDown]
    public void SaveCloudImageIfTestFailed()
    {
        var context = TestContext.CurrentContext;

        if (context.Result.Outcome.Status != TestStatus.Failed
            || layouter is null
            || layouter.Rectangles.Count == 0)
            return;


        var directory = Path.Combine(context.WorkDirectory, "FailedLayouts");
        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, $"{context.Test.Name}-{Guid.NewGuid():N}.png");

        CloudVisualizer.SaveImage(layouter.Rectangles, path);
        TestContext.Progress.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    [TestCase(0, 1)]
    [TestCase(-1, 1)]
    [TestCase(1, 0)]
    [TestCase(1, -1)]
    public void PutNextRectangle_WidthOrHeightAreNotPositive_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        var action = () => layouter!.PutNextRectangle(new Size(width, height));

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCaseSource(nameof(CenterPointCases))]
    public void PutNextRectangle_FirstRectangle_PutAtCenter(Point center)
    {
        layouter = CreateDefaultLayouter(center);
        var size = new Size(100, 50);

        var rectangle = layouter.PutNextRectangle(size);

        rectangle.Size.Should().Be(size);
        rectangle.Location.Should().Be(
            new Point(center.X - size.Width / 2, center.Y - size.Height / 2));
    }

    [TestCaseSource(nameof(CenterPointCases))]
    public void PutNextRectangle_ManyRectangles_CloudIsApproximatelyCentered(Point center)
    {
        const double centerTolerance = 10;

        layouter = CreateDefaultLayouter(center);

        var rectangles = GenerateRandomSizes(100, seed: 15)
            .Select(layouter!.PutNextRectangle)
            .ToList();

        var actualCenterX = (rectangles.Max(r => r.Right) + rectangles.Min(r => r.Left)) / 2.0;
        var actualCenterY = (rectangles.Max(r => r.Bottom) + rectangles.Min(r => r.Top)) / 2.0;

        actualCenterX.Should().BeApproximately(center.X, centerTolerance);
        actualCenterY.Should().BeApproximately(center.Y, centerTolerance);
    }

    [TestCaseSource(nameof(RectangleSizesCases))]
    public void PutNextRectangle_ManyRectangles_DoesNotIntersect(Size[] sizes)
    {
        var rectangles = sizes.Select(layouter!.PutNextRectangle).ToList();

        rectangles.Should().OnlyContain(rectangle => rectangles.Count(rectangle.IntersectsWith) == 1);
    }

    [TestCaseSource(nameof(RectangleSizesCases))]
    public void PutNextRectangle_ManyRectangles_CloudHasApproximatelyCircularShape(Size[] sizes)
    {
        const double maxWidthRatio = 1.2;

        var rectangles = sizes.Select(layouter!.PutNextRectangle).ToArray();

        var horizontalWidth = rectangles.Max(r => r.Right)
                              - rectangles.Min(r => r.Left);

        var verticalWidth = rectangles.Max(r => r.Bottom)
                            - rectangles.Min(r => r.Top);

        var diagonalScale = Math.Sqrt(2);

        var downRightWidth = (rectangles.Max(r => r.Right + r.Bottom)
                              - rectangles.Min(r => r.Left + r.Top)) / diagonalScale;

        var upRightWidth = (rectangles.Max(r => r.Right - r.Top)
                            - rectangles.Min(r => r.Left - r.Bottom)) / diagonalScale;

        double[] widths = [horizontalWidth, verticalWidth, downRightWidth, upRightWidth];

        var widthRatio = widths.Max() / widths.Min();

        widthRatio.Should().BeLessThanOrEqualTo(maxWidthRatio);
    }

    [TestCaseSource(nameof(RectangleSizesCases))]
    public void PutNextRectangle_ManyRectangles_CloudIsDense(Size[] sizes)
    {
        const double minDensity = 0.6;

        var rectangles = sizes.Select(layouter!.PutNextRectangle).ToList();

        var rectanglesArea = rectangles.Sum(r => r.Width * r.Height);

        var cloudArea = Math.PI * Math.Pow(
            rectangles.Max(r => GetMaxDistanceFromCenter(r, Point.Empty)), 2);

        var density = rectanglesArea / cloudArea;

        density.Should().BeGreaterThan(minDensity);
    }

    private static IEnumerable<Point> CenterPointCases()
    {
        yield return new Point(0, 0);
        yield return new Point(15, 80);
        yield return new Point(100, -50);
        yield return new Point(-666, 109);
        yield return new Point(-240, -35);
    }

    private static IEnumerable<TestCaseData> RectangleSizesCases()
    {
        const int count = 100;

        yield return new TestCaseData(
                Enumerable.Repeat(new Size(50, 50), count).ToArray())
            .SetArgDisplayNames("EqualSquares");

        yield return new TestCaseData(
                Enumerable.Repeat(new Size(100, 50), count).ToArray())
            .SetArgDisplayNames("EqualRectangles");

        yield return new TestCaseData(
                GenerateRandomSizes(count, seed: 0))
            .SetArgDisplayNames("RandomSizesWithSeed0");

        yield return new TestCaseData(
                GenerateRandomSizes(count, seed: 42))
            .SetArgDisplayNames("RandomSizesWithSeed42");
    }

    private static Size[] GenerateRandomSizes(int count, int seed, int minSize = 10, int maxSize = 100)
    {
        var random = new Random(seed);

        return Enumerable.Range(0, count)
            .Select(_ => new Size(
                random.Next(minSize, maxSize + 1),
                random.Next(minSize, maxSize + 1)))
            .ToArray();
    }

    private static double GetMaxDistanceFromCenter(Rectangle rect, Point center)
    {
        var dx = Math.Max(
            Math.Abs(rect.Right - center.X),
            Math.Abs(rect.Left - center.X));

        var dy = Math.Max(
            Math.Abs(rect.Bottom - center.Y),
            Math.Abs(rect.Top - center.Y));

        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static CircularCloudLayouter CreateDefaultLayouter(Point center)
    {
        var compactor = new RectangleCompactor();
        var spiral = new ArchimedeanSpiral(center);
        return new CircularCloudLayouter(center, spiral, compactor);
    }
}
