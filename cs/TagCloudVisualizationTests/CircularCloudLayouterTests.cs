using System.Drawing;
using FluentAssertions;
using TagCloudVisualization;

namespace TagCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Point center;
    private CircularCloudLayouter layouter;
    private List<Rectangle> placedRectangles;

    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        layouter = new CircularCloudLayouter(center);
        placedRectangles = new List<Rectangle>();
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            SaveFailedTestVisualization();

        placedRectangles.Clear();
    }

    [Test]
    public void CloudLayouterConstructor_ShouldCreateInstanceWithSpecificCenter()
    {
        layouter.Should().NotBeNull();
    }

    [Test]
    public void PutNextRectangle_ShouldAddFirstRectangleInCenter()
    {
        var rectangleSize = new Size(20, 10);
        
        var rectangle = layouter.PutNextRectangle(rectangleSize);
        placedRectangles.Add(rectangle);
        
        var expectedLocation = new Point(
            center.X - rectangleSize.Width / 2, 
            center.Y - rectangleSize.Height / 2);
        
        rectangle.Location.Should().Be(expectedLocation);
    }

    [Test]
    public void PutNextRectangle_ShouldAddTwoRectangles()
    {
        var firstRectangleSize = new Size(20, 10);
        var secondRectangleSize = new Size(10, 10);
        
        var firstRectangle = layouter.PutNextRectangle(firstRectangleSize);
        placedRectangles.Add(firstRectangle);
        
        var secondRectangle = layouter.PutNextRectangle(secondRectangleSize);
        placedRectangles.Add(secondRectangle);
        
        firstRectangle.Size.Should().Be(firstRectangleSize);
        secondRectangle.Size.Should().Be(secondRectangleSize);
        firstRectangle.Should().NotBe(secondRectangle);
    }

    [TestCase(-20, 10)]
    [TestCase(0, 0)]
    [TestCase(0, 1)]
    [TestCase(1, 0)]
    [TestCase(1, -10)]
    [TestCase(-10, -10)]
    public void PutNextRectangle_WithNonPositiveParameters_ShouldThrowArgumentException(int width, int height)
    {
        var rectangleSize = new Size(width, height);
        
        Action act = () => 
        {
            var rect = layouter.PutNextRectangle(rectangleSize);
            placedRectangles.Add(rect);
        };
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("The size of the rectangle must be a positive number.");
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ShouldNotIntersect()
    {
        var firstSize = new Size(30, 20);
        var secondSize = new Size(25, 15);
        
        var firstRect = layouter.PutNextRectangle(firstSize);
        placedRectangles.Add(firstRect);
        
        var secondRect = layouter.PutNextRectangle(secondSize);
        placedRectangles.Add(secondRect);
    
        firstRect.IntersectsWith(secondRect).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_ShouldNotIntersect()
    {
        var rectangles = new List<Rectangle>();
        
        for (int i = 1; i <= 10; i++)
        {
            var width = 10 + i * 2;
            var height = 10 + i;
            var rect = layouter.PutNextRectangle(new Size(width, height));
            rectangles.Add(rect);
            placedRectangles.Add(rect);
        }

        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse(
                    $"Rectangles {i} and {j} should not intersect");
            }
        }
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_ShouldFormCompactCloud()
    {
        var sizes = new[]
        {
            new Size(30, 20),
            new Size(10, 20),
            new Size(30, 10),
            new Size(15, 25)
        };
    
        var rectangles = sizes.Select(size => 
        {
            var rect = layouter.PutNextRectangle(size);
            placedRectangles.Add(rect);
            return rect;
        }).ToList();
        
        var bounds = GetBoundingRectangle(rectangles);
        var cloudCenter = new Point(
            bounds.Left + bounds.Width / 2, 
            bounds.Top + bounds.Height / 2);
        var distanceFromActualCenter = Distance(cloudCenter, center);
    
        distanceFromActualCenter.Should().BeLessThan(50, 
            "Cloud should be compact around the center");
    }
    
    [Test]
    public void PutNextRectangle_ManyRectangles_ShouldFormCircularShape()
    {
        var rectangleCount = 20;
        var maxAspectRatio = 1.5;
        var size = new Size(20, 10);
    
        var rectangles = new List<Rectangle>();
        for (int i = 0; i < rectangleCount; i++)
        {
            var rect = layouter.PutNextRectangle(size);
            rectangles.Add(rect);
            placedRectangles.Add(rect);
        }
    
        var bounds = GetBoundingRectangle(rectangles);
        var width = bounds.Width;
        var height = bounds.Height;
        
        var aspectRatio = (double)Math.Max(width, height) / Math.Min(width, height);
        aspectRatio.Should().BeLessThan(maxAspectRatio,
            $"Cloud with {rectangleCount} rectangles should be close to circular shape");
    }

    [Test]
    public void PutNextRectangle_ShouldWorkWithLargeNumberOfRectangles()
    {
        var rectangleCount = 50;
        var rectangles = new List<Rectangle>();

        for (int i = 0; i < rectangleCount; i++)
        {
            var width = 10 + i;
            var height = 8 + i * 2;
            var rect = layouter.PutNextRectangle(new Size(width, height));
            rectangles.Add(rect);
            placedRectangles.Add(rect);
        }

        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse(
                    $"Rectangles {i} and {j} should not intersect");
            }
        }
    }

    private void SaveFailedTestVisualization()
    {
        if (placedRectangles.Count == 0)
            return;
            
        var outputDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "FailedTests");
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        var testName = TestContext.CurrentContext.Test.Name;
        var safeTestName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
        var fileName = $"FailedTest_{safeTestName}.png";
        var filePath = Path.Combine(outputDir, fileName);

        var visualizer = new TagCloudVisualizer();
        visualizer.CreateVisualization(placedRectangles, center, filePath);

        TestContext.WriteLine($"Tag cloud visualization saved to file: {filePath}");
    }

    private Rectangle GetBoundingRectangle(List<Rectangle> rectangles)
    {
        if (rectangles.Count == 0)
            return Rectangle.Empty;

        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        var maxX = rectangles.Max(r => r.Right);
        var maxY = rectangles.Max(r => r.Bottom);

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    private double Distance(Point a, Point b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
}