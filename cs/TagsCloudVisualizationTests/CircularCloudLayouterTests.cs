using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Rectangle[] rectangles;
    private const string ImagesDirectory = "TestImages";

    private static readonly Point OptimalCenter = new(0, 0);

    [TearDown]
    public void TearDown()
    {
        var currentContext = TestContext.CurrentContext;
        if (currentContext.Result.Outcome.Status != TestStatus.Failed)
            return;

        var visualizer = new Visualizer();
        var bitmap = visualizer.CreateBitmap(rectangles, GetLayoutSize());
        Directory.CreateDirectory(ImagesDirectory);
        bitmap.Save(Path.Combine(ImagesDirectory, currentContext.Test.Name + ".jpg"), ImageFormat.Jpeg);

        TestContext.Out
            .WriteLine($"Tag cloud visualization saved to file " +
                       $"{Path.Combine(ImagesDirectory, currentContext.Test.Name + ".jpg")}");
    }

    [Test]
    public void CircularCloudLayouter_WhenCorrectArgs_NotThrowArgumentException()
    {
        var act = () => new CircularCloudLayouter(new Point(5, 15));

        act.Should().NotThrow<ArgumentException>();
    }

    [TestCase(-5, -5, TestName = "WithNegativeXAndY")]
    [TestCase(-5, 5, TestName = "WithNegativeX")]
    [TestCase(5, -5, TestName = "WithNegativeY")]
    public void CircularCloudLayouter_WhenIncorrectArgs_ThrowArgumentException(int x, int y)
    {
        var act = () => new CircularCloudLayouter(new Point(x, y));

        act.Should().Throw<ArgumentException>();
    }

    [TestCase(-5, -5, TestName = "WithNegativeWidthAndHeight")]
    [TestCase(-5, 5, TestName = "WithNegativeWidth")]
    [TestCase(5, -5, TestName = "WithNegativeHeight")]
    [TestCase(0, 0, TestName = "WithZeroWidthAndHeight")]
    public void PutNextRectangle_WhenIncorrectArgs_ThrowArgumentException(int width, int height)
    {
        var circularCloudLayouter = new CircularCloudLayouter(new Point(600, 600), 1, 90);

        var act = () => circularCloudLayouter.PutNextRectangle(new Size(width, height));

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectanglesWithoutIntersections()
    {
        var rectanglesNumber = 100;
        var circularCloudLayouter = new CircularCloudLayouter(OptimalCenter);

        rectangles = circularCloudLayouter.GenerateCloud(rectanglesNumber);

        IsIntersectionBetweenRectangles(rectangles).Should().BeFalse();
    }

    private static bool IsIntersectionBetweenRectangles(Rectangle[] rectangles)
    {
        for (var i = 0; i < rectangles.Length; i++)
        {
            for (var j = i + 1; j < rectangles.Length; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }

    [Test]
    public void TagsCloud_ShouldBeShapeOfCircularCloud_WhenOptimalParameters()
    {
        var rectanglesNumber = 1000;
        var circularCloudLayouter = new CircularCloudLayouter(OptimalCenter);

        rectangles = circularCloudLayouter.GenerateCloud(rectanglesNumber, 10, 25);
        var layoutSize = GetLayoutSize();
        var diametr = Math.Max(layoutSize.Height, layoutSize.Width);
        var circleArea = Math.PI* Math.Pow(diametr, 2) / 4;
        var rectanglesArea = (double)rectangles
            .Select(rectangle => rectangle.Height * rectangle.Width)
            .Sum();
        var accuracy = circleArea / rectanglesArea;

        accuracy.Should().BeApproximately(1, 0.35);
    }

    private Size GetLayoutSize()
    {
        var layoutWidth = rectangles.Max(rectangle => rectangle.Right) -
                          rectangles.Min(rectangle => rectangle.Left);
        var layoutHeight = rectangles.Max(rectangle => rectangle.Top) -
                           rectangles.Min(rectangle => rectangle.Bottom);
        return new Size(layoutWidth, layoutHeight);
    }

    [Test]
    public void TagsCloud_ShouldBeDense_WhenOptimalParameters()
    {
        var rectanglesNumber = 1000;
        var circularCloudLayouter = new CircularCloudLayouter(OptimalCenter);
        rectangles = circularCloudLayouter.GenerateCloud(rectanglesNumber, 10, 10);

        var expectedRectanglesArea = (double)rectanglesNumber * 10 * 10;
        var rectanglesArea = (double)rectangles
            .Select(rectangle => rectangle.Height * rectangle.Width)
            .Sum();
        var accuracy = expectedRectanglesArea / rectanglesArea;

        accuracy.Should().BeApproximately(1, 0.2);
    }
}