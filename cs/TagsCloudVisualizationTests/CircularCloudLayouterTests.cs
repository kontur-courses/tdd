using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagCloud;
using TagCloud.Visualisation;
using Color = SixLabors.ImageSharp.Color;

namespace TagCloudTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter cloudLayouter;
    private CloudVisualizer? visualizer;
    private Point center;

    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        cloudLayouter = new CircularCloudLayouter(center);
        
        var config = new CloudVisualizationConfigBuilder()
            .WithImageSize(800, 800)
            .WithBackground(CloudColor.Red)
            .WithCenter(new Point(400, 400))
            .WithRandomRectangleColors() 
            .Build();

        visualizer = new CloudVisualizer(config);
    }
    
    [TearDown]
    public void TearDown()
    {
        var context = TestContext.CurrentContext;
        
        if (context.Result.Outcome.Status != TestStatus.Failed)
            return;

        if (cloudLayouter.Rectangles.Count == 0)
            return;
        var now = DateTime.Now;
        var relativeFileName = Path.Combine(
            "Failures",
            $"{context.Test.Name}_{now:yyyyMMdd_HHmmss}_{now.Ticks}.png");
        
        visualizer?.DrawLayout(cloudLayouter.Rectangles, relativeFileName);
        
        var projectRoot = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var fullPath = Path.Combine(projectRoot, relativeFileName);
        
        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {fullPath}");
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public void PutNextRectangle_WidthLessThanOne_ShouldThrowForWidth(int width)
    {
        var rectangleSize = new Size(width, 10);

        var act = () => cloudLayouter.PutNextRectangle(rectangleSize);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Ширина прямоугольника должна быть положительной (Parameter 'Width')");
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public void PutNextRectangle_HeightLessThanOne_ShouldThrowForHeight(int height)
    {
        var rectangleSize = new Size(10, height);

        var act = () => cloudLayouter.PutNextRectangle(rectangleSize);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Высота прямоугольника должна быть положительной (Parameter 'Height')");
    }

    [Test]
    public void PutNextRectangle_WithValidSize_ShouldNotThrow()
    {
        var rectangleSize = new Size(10, 20);

        var act = () => cloudLayouter.PutNextRectangle(rectangleSize);

        act.Should().NotThrow();
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBePlacedInCenter()
    {
        var size = new Size(20, 10);

        var rect = cloudLayouter.PutNextRectangle(size);

        GetCenter(rect).Should().Be(center);
        rect.Size.Should().Be(size);

        cloudLayouter.Rectangles.Should().ContainSingle();
        cloudLayouter.Rectangles.Single().Should().Be(rect);
    }
    
    [Test]
    public void PutNextRectangle_ManyRectangles_ShouldPutCorrectNumber()
    {
        var size = new Size(30, 15);
        var rectangleCount = 0;

        for (var i = 0; i < 50; i++)
        {
            cloudLayouter.PutNextRectangle(size);
            rectangleCount++;
        }

        cloudLayouter.Rectangles.Count.Should().Be(rectangleCount);
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_ShouldNotIntersect()
    {
        var size = new Size(30, 15);
        var rectangles = new List<Rectangle>();

        for (var i = 0; i < 50; i++)
            rectangles.Add(cloudLayouter.PutNextRectangle(size));

        for (var i = 0; i < rectangles.Count; i++)
        for (var j = i + 1; j < rectangles.Count; j++)
        {
            rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }
    }
    
    [Test]
    [Repeat(100)]
    public void PutNextRectangle_CloudForManyRectangles_ShouldBeDense()
    {
        var random = new Random(42);
        
        for (var i = 0; i < 300; i++)
        {
            var size = new Size(
                width: random.Next(15, 20),
                height: 7
            );
            
            cloudLayouter.PutNextRectangle(size);
        }

        var density = GetDensity(cloudLayouter.Rectangles);
        
        density.Should().BeGreaterThan(0.75);
    }
    
    // для проверки визуализации падений тестов
    [Test]
    [Explicit]
    public void PutNextRectangle_GenerateRandomCloudAndFail()
    {
        var random = new Random(42);
        
        for (var i = 0; i < 30; i++)
        {
            var size = new Size(
                width: random.Next(10, 50),
                height: random.Next(10, 50));
            
            cloudLayouter.PutNextRectangle(size);
        }
        
        Assert.Fail();
    }

    private double GetDensity(IReadOnlyCollection<Rectangle> rectangles)
    {
        if (rectangles.Count == 0)
            return 0;
        
        var areaRects = rectangles.Sum(r => (double)r.Width * r.Height);
        
        var maxDistSquared = 0.0;

        foreach (var r in rectangles)
        {
            var corners = new[]
            {
                new Point(r.Left,  r.Top),
                new Point(r.Right, r.Top),
                new Point(r.Left,  r.Bottom),
                new Point(r.Right, r.Bottom)
            };

            foreach (var p in corners)
            {
                var dx = p.X - center.X;
                var dy = p.Y - center.Y;
                var distSq = (double)dx * dx + dy * dy;
                if (distSq > maxDistSquared)
                    maxDistSquared = distSq;
            }
        }

        if (maxDistSquared <= 0)
            return 0;

        var radius = Math.Sqrt(maxDistSquared);
        var circleArea = Math.PI * radius * radius;

        return areaRects / circleArea;
    }

    private static Point GetCenter(Rectangle rect) =>
        new(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
}
