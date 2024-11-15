using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private const int IMAGE_SIZE = 800;
    private List<Rectangle> rectanglesInTest;

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            return;

        var directory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "FailedVisualisations");
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, $"{TestContext.CurrentContext.Test.Name}_visualisation.png");

        var visuliser = new CloudVisualiser();
        visuliser.VisualiseAndSave(rectanglesInTest, path, new(IMAGE_SIZE, IMAGE_SIZE));
        Console.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    [TestCase(0, 1, TestName = "Zero width")]
    [TestCase(1, 0, TestName = "Zero height")]
    [TestCase(-1, 1, TestName = "Negative width")]
    [TestCase(1, -1, TestName = "Negative height")]
    public void Layouter_ThrowArgumentException_WithUncorrectData(int width, int height)
    {
        var layouter = new CircularCloudLayouter(new(0, 0));
        var size = new Size(width, height);
        Action action = () => layouter.PutNextRectangle(size);

        action.Should().Throw<ArgumentException>();
    }

    [TestCase(0, 0, TestName = "Zero center")]
    [TestCase(1, 1, TestName = "Non-zero center")]
    public void LayouterPutFirstRectangleInCenter(int x, int y)
    {
        var layouter = new CircularCloudLayouter(new(x, y));
        var rectangleSize = new Size(10, 10);

        var actualRectangle = layouter.PutNextRectangle(rectangleSize);
        var expectedRectangle = new Rectangle(
            -rectangleSize.Width / 2 + x,
            -rectangleSize.Height / 2 + y,
            rectangleSize.Width,
            rectangleSize.Height
        );
        rectanglesInTest = layouter.GetRectangles();

        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    [Repeat(5)]
    public void RectanglesHaveNotIntersects()
    {
        var random = new Random();
        var count = random.Next(10, 30);

        var layouter = CloudGenerator.GenerateRandomCloudWithCenter(new(0, 0), count);
        rectanglesInTest = layouter.GetRectangles();

        AreRectanglesHaveIntersects(layouter.GetRectangles()).Should().BeFalse();
    }

    [Test]
    [Repeat(5)]
    public void RectanglesCenterShoulBeLikeInitCenter()
    {
        var center = new Point(0, 0);
        var treshold = 25;
        var layouter = CloudGenerator.GenerateRandomCloudWithCenter(center, 100);
        rectanglesInTest = layouter.GetRectangles();

        var actualCenter = GetCenterOfRectangles(layouter.GetRectangles());

        actualCenter.X.Should().BeInRange(center.X - treshold, center.X + treshold);
        actualCenter.Y.Should().BeInRange(center.Y - treshold, center.Y + treshold);
    }

    [Test]
    [Repeat(5)]
    public void RectanglesDensityShouldBeMaximum()
    {
        var expectedDensity = 0.45;
        var center = new Point(0, 0);
        var layouter = CloudGenerator.GenerateRandomCloudWithCenter(center, 100);
        rectanglesInTest = layouter.GetRectangles();

        var rectanglesArea = layouter.GetRectangles().Sum(r => r.Width * r.Height);
        var radius = GeMaxDistanceFromRectangleToCenter(layouter.GetRectangles());
        var circleArea = Math.PI * radius * radius;
        var density = rectanglesArea / circleArea;

        density.Should().BeGreaterThanOrEqualTo(expectedDensity);

    }

    private double GeMaxDistanceFromRectangleToCenter(List<Rectangle> rectangles)
    {
        var center = GetCenterOfRectangles(rectangles);

        double maxDistance = 0;

        foreach (var rectangle in rectangles)
        {
            var corners = new Point[4]
            {
                new(rectangle.Top, rectangle.Left),
                new(rectangle.Top, rectangle.Right),
                new(rectangle.Bottom, rectangle.Left),
                new(rectangle.Bottom, rectangle.Right)
            };

            var distance = corners.Max(p => GetDistanceBetweenPoints(p, center));
            maxDistance = Math.Max(maxDistance, distance);
        }

        return maxDistance;
    }

    private Point GetCenterOfRectangles(List<Rectangle> rectangles)
    {
        var top = rectangles.Max(r => r.Top);
        var left = rectangles.Min(r => r.Left);
        var bottom = rectangles.Min(r => r.Bottom);
        var right = rectangles.Max(r => r.Right);

        var x = left + (right - left) / 2;
        var y = bottom + (top - bottom) / 2;

        return new(x, y);
    }

    private bool AreRectanglesHaveIntersects(List<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }

    private double GetDistanceBetweenPoints(Point point1, Point point2)
        => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
}