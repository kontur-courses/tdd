using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterTests
{
    private ILayouter layouter;
    private Point center;
    private TagCloudVisualizer testVisualizer;
    private IImageSaver imageSaver;
    private List<Rectangle> placedRectangles;

    [SetUp]
    public void SetUp()
    {
        center = new Point(100, 100);
        layouter = new CircularCloudLayouter(center, new SpiralPointsProvider(center));
        imageSaver = new ImageSaver("test_results");
        testVisualizer = new TagCloudVisualizer(imageSaver);
        placedRectangles = [];
    }
    
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) 
            return;
        try
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{testName}_{timestamp}.png";
            var config = new TagCloudVisualizationConfig(fileName);
            
            var filePath = testVisualizer.SaveVisualization(
                placedRectangles, 
                center, 
                config
            );
            
            TestContext.Out.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Failed to save visualization: {ex.Message}");
        }
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_PlacedInCenter()
    {
        var size = new Size(10, 10);
        
        var actual = layouter.PutNextRectangle(size);
        placedRectangles.Add(actual);
        
        var expected = new Rectangle(
            new Point(center.X - size.Width / 2, center.Y - size.Height / 2), 
            size);
        actual.Should().Be(expected);
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_DoNotIntersect()
    {
        for (var i = 0; i < 5; i++)
        {
            var rectangle = layouter.PutNextRectangle(new Size(45, 15));
            placedRectangles.Add(rectangle);
        }
        
        placedRectangles.Should().NotBeEmpty();
        
        for (var i = 0; i < placedRectangles.Count; i++)
            for (var j = i + 1; j < placedRectangles.Count; j++)
                placedRectangles[i].IntersectsWith(placedRectangles[j]).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_LayoutIsDense()
    {
        var sizes = new[]
        {
            new Size(60, 20),
            new Size(45, 15),
            new Size(30, 10),
            new Size(20, 40),
            new Size(15, 5)
        };
        
        var random = new Random(42);
        for (var i = 0; i < 200; i++)
        {
            var size = sizes[random.Next(sizes.Length)];
            var rectangle = layouter.PutNextRectangle(size);
            placedRectangles.Add(rectangle);
        }
    
        const double minDensityRatio = 0.6;
    
        var totalRectanglesArea = placedRectangles
            .Sum(rectangle => rectangle.Width * rectangle.Height);
        
        var boundingCircleRadius = CalculateBoundingCircleRadius(placedRectangles);
        var boundingCircleArea = Math.PI * boundingCircleRadius * boundingCircleRadius;
        
        var density = totalRectanglesArea / boundingCircleArea;
        TestContext.Out.WriteLine($"Actual density: {density:F3}");
        density.Should().BeGreaterThan(minDensityRatio);
    }
    
    [Test]
    public void PutNextRectangle_ManyRectangles_RectanglesInAllQuadrants()
    {
        for (var i = 0; i < 150; i++)
        {
            var rectangle = layouter.PutNextRectangle(new Size(45, 15));
            placedRectangles.Add(rectangle);
        }

        var quadrants = new int[4];

        foreach (var rect in placedRectangles)
        {
            var rectCenter = new Point(rect.X + rect.Width / 2,
                rect.Y + rect.Height / 2);

            if (rectCenter.X >= center.X && rectCenter.Y <= center.Y)
                quadrants[0]++;
            else if (rectCenter.X >= center.X && rectCenter.Y > center.Y)
                quadrants[1]++;
            else if (rectCenter.X < center.X && rectCenter.Y > center.Y)
                quadrants[2]++;
            else
                quadrants[3]++;
        }
        
        quadrants.All(count => count > 0).Should().BeTrue(
            "Прямоугольники должны распределиться по всем квадрантам, образуя круглую форму");
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_DistributionIsRelativelyUniform()
    {
        for (var i = 0; i < 150; i++)
        {
            var rectangle = layouter.PutNextRectangle(new Size(45, 15));
            placedRectangles.Add(rectangle);
        }
        
        var quadrants = new int[4];
    
        foreach (var rect in placedRectangles)
        {
            var rectCenter = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            
            if (rectCenter.X >= center.X && rectCenter.Y <= center.Y) 
                quadrants[0]++;
            else if (rectCenter.X >= center.X && rectCenter.Y > center.Y) 
                quadrants[1]++;
            else if (rectCenter.X < center.X && rectCenter.Y > center.Y) 
                quadrants[2]++;
            else 
                quadrants[3]++;
        }
        
        var maxCount = quadrants.Max();
        var minCount = quadrants.Min();
        
        (maxCount <= minCount * 2).Should().BeTrue("Распределение по квадрантам должно быть относительно равномерным");
        
        TestContext.Out.WriteLine($"Распределение по квадрантам: [{string.Join(", ", quadrants)}]");
        TestContext.Out.WriteLine($"Максимум: {maxCount}, Минимум: {minCount}, Отношение: {(double)maxCount / minCount:F2}");
    }

    private double CalculateBoundingCircleRadius(List<Rectangle> rectangles)
    {
        return rectangles
            .SelectMany(rect => new[]
            {
                GetDistance(rect.Location, center),
                GetDistance(new Point(rect.Right, rect.Top), center),
                GetDistance(new Point(rect.Left, rect.Bottom), center),
                GetDistance(new Point(rect.Right, rect.Bottom), center)
            })
            .Max();
    }

    private double GetDistance(Point point1, Point point2)
    {
        return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
    }
}