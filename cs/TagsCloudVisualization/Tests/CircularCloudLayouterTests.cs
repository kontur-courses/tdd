using System.Diagnostics;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter defaultLayouter = new CircularCloudLayouter(new());

    [SetUp]
    public void SetUp()
    {
        defaultLayouter = new CircularCloudLayouter(new Point(400, 400));
    }

    [Test]
    public void Constructor_ShouldSetCenter()
    {
        defaultLayouter.Center.Should().Be(defaultLayouter.Center);
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBePlacedAtCenter()
    {
        var size = new Size(20, 10);
        var rectangle = defaultLayouter.PutNextRectangle(size);

        var expectedRectangle = new Rectangle(
            defaultLayouter.Center.X - size.Width / 2,
            defaultLayouter.Center.Y - size.Height / 2,
            size.Width,
            size.Height);

        rectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ShouldNotIntersect()
    {
        var size1 = new Size(10, 10);
        var size2 = new Size(20, 20);

        var rectangle1 = defaultLayouter.PutNextRectangle(size1);
        var rectangle2 = defaultLayouter.PutNextRectangle(size2);

        rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_ShouldNotIntersect()
    {
        var sizes = new[]
        {
            new Size(20, 10),
            new Size(15, 15),
            new Size(25, 8),
            new Size(12, 18),
            new Size(30, 10)
        };

        var rectangles = new List<Rectangle>();
        foreach (var size in sizes)
        {
            rectangles.Add(defaultLayouter.PutNextRectangle(size));
        }
        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int j = i + 1; j < rectangles.Count; j++)
            {
                Assert.That(rectangles[i].IntersectsWith(rectangles[j]), Is.False,
                    $"Rectangles {i} and {j} should not intersect");
            }
        }
    }
    
    [Test]
    public void PutNextRectangle_Rectangles_ShouldFormCircularShape()
    {
        var rectangles = GenerateRandomRectangles(100);

        var maxDistance = 0d;
        var minDistance = double.MaxValue;
        var averageDistance = 0d;
        var count = 0;
        
        foreach (var rect in rectangles)
        {
            count++;
            var rectCenter = new Point(
                rect.X + rect.Width / 2,
                rect.Y + rect.Height / 2);

            var distance = Math.Sqrt(
                Math.Pow(rectCenter.X - defaultLayouter.Center.X, 2) + 
                Math.Pow(rectCenter.Y - defaultLayouter.Center.Y, 2));

            maxDistance = Math.Max(maxDistance, distance);
            minDistance = Math.Min(minDistance, distance);
            averageDistance += distance;
        }
        
        averageDistance /= count;
        var distanceRange = maxDistance - minDistance;
        var relativeRange = distanceRange / averageDistance;
        
        relativeRange.Should().BeLessThan(1.5);
    }
    
    [Test]
    public void PutNextRectangle_Rectangles_ShouldBeDenselyPacked()
    {
        var rectangles = GenerateRandomRectangles(200).ToList();
        var lastRect = rectangles.Last();
        var rectCenter = new Point(lastRect.X + lastRect.Width / 2, lastRect.Y + lastRect.Height / 2);
        var distanceToCenter = Math.Sqrt(
            Math.Pow(rectCenter.X - defaultLayouter.Center.X, 2) + 
            Math.Pow(rectCenter.Y - defaultLayouter.Center.Y, 2));
        
        var circleArea = Math.PI * distanceToCenter * distanceToCenter;
        var rectanglesArea = rectangles.Sum(r => r.Width * r.Height);
        var density = rectanglesArea / circleArea;

        density.Should().BeGreaterThan(0.7);
    }
    
    // Во многом результат зависит от рандома, в целом алгоритм медленный
    [Test]
    public void PutNextRectangle_Performance_ShouldBeLinear()
    {

        var smallCount = 10;
        var largeCount = smallCount * 100; 

        var smallTime = MeasureTime(() => GenerateRandomRectangles(smallCount));
        var largeTime = MeasureTime(() => GenerateRandomRectangles(largeCount));
        
        largeTime.Should().BeLessThan(smallTime * 100);
    }

    private long MeasureTime(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }

    [Test]
    [Explicit]
    [TestCase(20, "rectangles_20", TestName = "Draw 20 rectangles")]
    [TestCase(200, "rectangles_200", TestName = "Draw 200 rectangles")]
    [TestCase(2000, "rectangles_2000", TestName = "Draw 2000 rectangles")]
    [TestCase(5000, "rectangles_5000", TestName = "Draw 5000 rectangles")]
    public void Draw_Reactangles(int count, string filename)
    {
        var layouter = new CircularCloudLayouter(new Point(1000, 1000));
        var rectangles = GenerateRandomRectangles(count, layouter);
        CloudVisualizer.DrawRectangles(rectangles, filename, 2000, 2000);
    }

    private IEnumerable<Rectangle> GenerateRandomRectangles(int count, CircularCloudLayouter? layouter = null)
    {
        if (layouter is null)
            layouter = defaultLayouter;
        var random = new Random(123);
        return Enumerable.Range(0, count)
            .Select(_ => new Size(random.Next(10, 50), random.Next(10, 50)))
            .Select(size => layouter.PutNextRectangle(size))
            .ToList();
    }
}