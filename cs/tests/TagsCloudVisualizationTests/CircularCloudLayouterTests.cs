using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TagsCloudVisualization.Base;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualizationTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class CircularCloudLayouterTests
{
    private const int Seed = 232445332;
    private static readonly Randomizer Random = new(Seed);
    private Rectangle[] testRectangles = null!;
    private const string ImagesDirectory = "testImages";

    [TearDown]
    public void TearDown()
    {
        var currentContext = TestContext.CurrentContext;
        if (currentContext.Result.Outcome.Status != TestStatus.Failed)
            return;
        
        var rectanglesWindow = new RectanglesWindow(testRectangles);
        var visualizer = new CartesianVisualizer(new Size(rectanglesWindow.Width, rectanglesWindow.Height));
        using var bitmap = visualizer.CreateBitmap(testRectangles);
        
        var path = Path.Combine(ImagesDirectory, currentContext.Test.Name + ".png");
        Directory.CreateDirectory(ImagesDirectory);
        bitmap.Save(path, ImageFormat.Png);
        
        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesInCenter()
    {
        var center = Random.NextPoint(-100, 100);
        testRectangles = PutRectanglesInCloudLayouter(center);

        var actualRectanglesWindow = new RectanglesWindow(testRectangles);
        var centerOffset = center.Subtract(actualRectanglesWindow.Center).Abs();
        var xError = testRectangles.Max(r => r.Width);
        var yError = testRectangles.Max(r => r.Height);

        using (new AssertionScope())
        {
            centerOffset.X.Should().BeLessThanOrEqualTo(xError);
            centerOffset.Y.Should().BeLessThanOrEqualTo(yError);
        }
    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesInCircle()
    {
        var center = Random.NextPoint(-100, 100);
        testRectangles = PutRectanglesInCloudLayouter(center);
        
        var actualRectanglesWindow = new RectanglesWindow(testRectangles);
        var radius = (actualRectanglesWindow.Width + actualRectanglesWindow.Height) / 4;
        var actualSquare = (double)testRectangles.Sum(r => r.Width * r.Height);
        var expectedSquare = PolarMath.GetSquareOfCircle(radius);

        const double allowableSquareFraction = 0.275;
        var precision = expectedSquare * allowableSquareFraction;
        actualSquare.Should().BeApproximately(expectedSquare, precision);
    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnDontIntersectsRectangles()
    {
        var center = Random.NextPoint(-100, 100);
        testRectangles = PutRectanglesInCloudLayouter(center);
        
        IsHaveIntersects(testRectangles).Should().BeFalse();
    }
    
    private static Rectangle[] PutRectanglesInCloudLayouter(Point center)
    {
        var rectanglesCount = Random.Next(100, 500);
        var circularCloudLayouter = new CircularCloudLayouter(center, 1, 0.5);

        return Enumerable
            .Range(0, rectanglesCount)
            .Select(_ => Random.NextSize(10, 50))
            .Select(s => circularCloudLayouter.PutNextRectangle(s))
            .ToArray();
    }

    private static bool IsHaveIntersects(Rectangle[] rectangles)
    {
        for (var i = 0; i < rectangles.Length; i++)
            for (var j = i + 1; j < rectangles.Length; j++) 
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
        
        return false;
    }
}