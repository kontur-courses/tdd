using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class CircularCloudLayouterTest
{
    private CircularCloudLayouter cloudLayouter;

    [SetUp]
    public void Setup()
    {
        cloudLayouter = new CircularCloudLayouter(new Point(0, 0));
    }

    [TestCase(0, 5, TestName = "zero width")]
    [TestCase(5, 0, TestName = "zero height")]
    [TestCase(-5, 5, TestName = "negative width")]
    [TestCase(5, -5, TestName = "negative height")]
    public void PutNextRectangle_ThrowsArgumentException_OnInvalidRectangleSize(int width, int height)
    {
        var rectangleSize = new Size(width, height);

        var action = () => cloudLayouter.PutNextRectangle(rectangleSize);

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_RectangleDoesNotIntersect()
    {
        var rectangles = PutRandomRectangles(50);

        var intersect = rectangles.Any(rectangle => rectangles
            .Where(otherRectangle => otherRectangle != rectangle)
            .Any(rectangle.IntersectsWith));
        
        intersect.Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_RectanglesShouldBeLocatedInAlmostCircle()
    {
        var rectangles = PutRandomRectangles(50);

        var radius = CalculateRadius(rectangles);
        var inCircle = rectangles.ToList().FindAll(rectangle =>
            CalculateDistance(cloudLayouter.Center(), rectangle.Location) < radius);
        var entryFactor = (double)inCircle.Count / rectangles.Count;

        entryFactor.Should().BeGreaterThan(0.8);
    }

    [Test]
    public void PutNextRectangle_RectanglesShouldBeLocatedDensely()
    {
        var rectangles = PutRandomRectangles(1000);

        var radius = CalculateRadius(rectangles);
        var sumSizes = rectangles.Sum(rectangle => rectangle.Size.Width * rectangle.Size.Height);
        var circleSquare = Math.PI * Math.Pow(radius, 2);
        var density = sumSizes / circleSquare;

        density.Should().BeGreaterThan(0.6);
    }

    private static int CalculateRadius(IReadOnlyCollection<Rectangle> rectangles)
    {
        var minX = rectangles.Min(r => r.Left);
        var maxX = rectangles.Max(r => r.Right);
        var minY = rectangles.Min(r => r.Top);
        var maxY = rectangles.Max(r => r.Bottom);

        return Math.Max(maxX - minX, maxY - minY) / 2;
    }

    private static double CalculateDistance(Point firstPoint, Point secondPoint)
    {
        return Math.Sqrt(Math.Pow(secondPoint.X - firstPoint.X, 2) + Math.Pow(secondPoint.Y - firstPoint.Y, 2));
    }

    private IReadOnlyCollection<Rectangle> PutRandomRectangles(int amount)
    {
        var random = new Random();
        for (var i = 0; i < amount; i++)
        {
            var rectangleSize = new Size(random.Next(10, 50), random.Next(10, 50));
            cloudLayouter.PutNextRectangle(rectangleSize);
        }

        return cloudLayouter.Rectangles();
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
        using var bitmap = CloudImageGenerator.Generate(cloudLayouter);
        CloudImageSaver.SaveFailedTest(bitmap, TestContext.CurrentContext.Test.Name);
    }
}