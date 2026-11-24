using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloudVisualization.Visualization;

namespace TagCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouter_Should
{
    private const double MinDensity = 0.6;

    private const int CenterOffsetTolerance = 50;
    
    private const double AspectRatioTolerance = 0.2;
    
    private static readonly Point Center = new(1000, 1000);
    
    private CircularCloudLayouter layouter;
    
    [SetUp]
    public void SetUp() => layouter = new CircularCloudLayouter(Center);
    
    [TearDown]
    public void SaveVisualizationOnFailure()
    {
        var context = TestContext.CurrentContext;
        
        if (context.Result.Outcome.Status != TestStatus.Failed) 
            return;
        
        if (layouter.Rectangles.Count == 0) 
            return;

        var path = Path.Combine(context.WorkDirectory, $"{context.Test.Name}.png");

        var image = CloudRenderer.Render(layouter.Rectangles);
        CloudImageSaver.Save(image, path);

        Console.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_IsPlacedAtCloudCenter()
    {
        var size = new Size(100, 60);

        var rect = layouter.PutNextRectangle(size);

        var cx = rect.Left + rect.Width / 2.0;
        var cy = rect.Top + rect.Height / 2.0;

        cx.Should().Be(Center.X);
        cy.Should().Be(Center.Y);
    }

    [Test]
    public void PutNextRectangle_ReturnedRectangle_HasRequestedSize()
    {
        var size = new Size(120, 60);
        
        var rectangle = layouter.PutNextRectangle(size);
        
        rectangle.Width.Should().Be(size.Width);
        rectangle.Height.Should().Be(size.Height);
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_DoNotIntersect()
    {
        var sizes = new[]
        {
            new Size(100, 50),
            new Size(80, 40),
            new Size(60, 60),
            new Size(120, 30),
            new Size(40, 90),
            new Size(70, 70),
            new Size(90, 40),
            new Size(50, 50)
        };
        
        var rectangles = sizes
            .Select(layouter.PutNextRectangle)
            .ToArray();
        
        for (var i = 0; i < rectangles.Length; i++)
        for (var j = i + 1; j < rectangles.Length; j++)
        {
            rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_ProducesApproximatelyCircularCloud()
    {
        var sizes = Enumerable
            .Range(0, 40)
            .Select(i => new Size(50, 50))
            .ToArray();
        
        var rectangles = sizes
            .Select(layouter.PutNextRectangle)
            .ToArray();
        
        var bounds = GetBounds(rectangles);

        var aspectRatio = (double)bounds.Width / bounds.Height;

        aspectRatio.Should().BeApproximately(1, AspectRatioTolerance);
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_ProducesCompactCloud()
    {
        var sizes = Enumerable
            .Range(0, 40)
            .Select(i => new Size(50, 50))
            .ToArray();

        var rectangles = sizes
            .Select(layouter.PutNextRectangle)
            .ToArray();

        var rectanglesArea = rectangles.Sum(
            r => (double)r.Width * r.Height);

        var bounds = GetBounds(rectangles);

        var boundingArea = (double)bounds.Width * bounds.Height;

        var density = rectanglesArea / boundingArea;

        density.Should().BeGreaterThan(MinDensity);
    }

    [Test]
    public void PutNextRectangle_CloudIsCentered_AroundRequestedCenter()
    {
        var sizes = Enumerable
            .Range(0, 40)
            .Select(i => new Size(50, 50))
            .ToArray();
        
        var rectangles = sizes
            .Select(layouter.PutNextRectangle)
            .ToArray();
        
        var bounds = GetBounds(rectangles);
        
        var boundsCenterX = bounds.Left + bounds.Width / 2.0;
        var boundsCenterY = bounds.Top + bounds.Height / 2.0;

        boundsCenterX.Should().BeApproximately(Center.X, CenterOffsetTolerance);
        boundsCenterY.Should().BeApproximately(Center.Y, CenterOffsetTolerance);
    }
    
    [Test]
    public void PutNextRectangle_ZeroSizedRectangle_ProducesEmptyRectangle()
    {
        var rect = layouter.PutNextRectangle(new Size(0, 0));

        rect.Size.Should().Be(Size.Empty);
    }

    private static Rectangle GetBounds(Rectangle[] rectangles)
    {
        var left = rectangles.Min(r => r.Left);
        var top = rectangles.Min(r => r.Top);
        var right = rectangles.Max(r => r.Right);
        var bottom = rectangles.Max(r => r.Bottom);

        return new Rectangle(
            left,
            top,
            right - left,
            bottom - top);
    }
}