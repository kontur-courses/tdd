using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloudVisualizer.CloudLayouter;
using TagCloudVisualizer.TagCloudImageGenerator;

namespace TagCloudVisualiser_Tests;

[TestFixture]
public class CircularCloudLayouter_Tests
{
    private static readonly Size CANVAS_SIZE = new Size(250, 250);
    private static readonly Point CENTER = new Point(CANVAS_SIZE.Width / 2, CANVAS_SIZE.Height / 2);

    private static Dictionary<string, CircularCloudLayouter> Layouters = new ();
    private static Dictionary<string, Func<Size>> SizeSelectors = new ();
    private static Dictionary<string, List<Rectangle>> RectangleLists = new ();

    private static void CreateLayouter(Func<Size> sizeSelector)
    {
        var name = TestContext.CurrentContext.Test.FullName;
        Layouters[name] = new CircularCloudLayouter(CENTER);
        SizeSelectors[name] = sizeSelector;
        RectangleLists[name] = new List<Rectangle>();
    } 
    
    private static Rectangle GetNextRectangle()
    {
        var name = TestContext.CurrentContext.Test.FullName;
        var rectangleSize = SizeSelectors[name]();
        var rectangle = Layouters[name].PutNextRectangle(rectangleSize);
        RectangleLists[name].Add(rectangle);
        return rectangle;
    } 

    [TearDown]
    public void TearDownTest()
    {
        if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
            return;
    
        var path = Environment.CurrentDirectory;
        var folderName = "FailedTests";
        var timestamp = DateTime.Now;
        var filename = $"{TestContext.CurrentContext.Test.Name}_{timestamp:yyyy-MM-dd-HH-mm-ss}.png";
    
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        var fullFilePath = Path.Combine(path, folderName, filename);
        var rectangles = RectangleLists[TestContext.CurrentContext.Test.FullName];
        
        var image = TagCloudImageGenerator.GenerateImage(rectangles.ToArray(), CANVAS_SIZE);
        image.Save(fullFilePath, ImageFormat.Png);
        
        Console.WriteLine($"Image of generated tag cloud saved as {fullFilePath}");
    }
    
    [TestCase(0, 0, Description = "Zero coordinates")]
    [TestCase(1, 1, Description = "Positive coordinates")]
    [TestCase(-1, -1, Description = "Negative coordinates")]
    public void Constructor_ShouldAcceptAnyPoint(int x, int y)
    {
        Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(x, y)));
    }

    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    public void PutNextRectangle_ShouldThrowArgumentException_OnNegativeSize(int width, int height)
    {
        CreateLayouter(() => new Size(width, height));
        Assert.Throws<ArgumentException>(() => GetNextRectangle());
    }

    [Test]
    public void PutNextRectangle_ShouldPlaceFirstRectangleInCenter()
    {
        var expectedSize = new Size(10, 10);
        var expectedLocation = new Point(CENTER.X - expectedSize.Width / 2, CENTER.Y - expectedSize.Height / 2);
        var expectedRectangle = new Rectangle(expectedLocation, expectedSize);

        CreateLayouter(() => new Size(10, 10));
        var actualRectangle = GetNextRectangle();
        
        Assert.That(actualRectangle, Is.EqualTo(expectedRectangle));
    }
    
    [Test(Description = "A thousand rectangles should not intersect")]
    public void PutNextRectangle_RectanglesShouldNotIntersect_WithSameRectangles()
    {
        var sizeSelector = () => new Size(10, 10);
        AssertRectanglesDontIntersect(1000, sizeSelector);
    }
    
    [Test(Description = "A thousand rectangles should not intersect")]
    public void PutNextRectangle_RectanglesShouldNotIntersect_WithRandomRectangles()
    {
        var random = new Random(123);
        var sizeSelector = () => new Size(random.Next(1, 11), random.Next(1, 11));
        AssertRectanglesDontIntersect(1000, sizeSelector);
    }
    
    [TestCase(Description = "A thousand rectangles should create a circle")]
    public void PutNextRectangle_ShouldCreateATightCircleAroundCenter_WithSameRectangles()
    {
        var sizeSelector = () => new Size(10, 10);
        AssertShapeIsATightCircleAroundCenter(1000, sizeSelector);
    }
    
    [TestCase(Description = "A thousand rectangles should create a circle")]
    public void PutNextRectangle_ShouldCreateATightCircleAroundCenter_WithRandomRectangles()
    {
        var random = new Random(123);
        var sizeSelector = () => new Size(random.Next(1, 11), random.Next(1, 11));

        AssertShapeIsATightCircleAroundCenter(1000, sizeSelector);
    }

    private void AssertRectanglesDontIntersect(int count, Func<Size> rectangleSizeSelector)
    {
        CreateLayouter(rectangleSizeSelector);
        
        var checkedRectangles = new List<Rectangle>();
        for (var i = 0; i < count; i++)
        {
            var rectangle = GetNextRectangle();
            if (checkedRectangles.Any(r => r.IntersectsWith(rectangle)))
                Assert.Fail($"Rectangles should not intersect, failed on iteration {i + 1}");
            checkedRectangles.Add(rectangle);
        }
    }
    
    private void AssertShapeIsATightCircleAroundCenter(int count, Func<Size> rectangleSizeSelector)
    {
        CreateLayouter(rectangleSizeSelector);
        var layout = new List<Rectangle>();
        for (var i = 0; i < count; i++)
            layout.Add(GetNextRectangle());

        var areaCoveredByRectangles = GetAreaCoveredByRectangles(layout);
        var (upperRadius, lowerRadius, rightRadius, leftRadius) = GetRadii(layout);

        var verticalDiameter = upperRadius + lowerRadius;
        var horizontalDiameter = rightRadius + leftRadius;
        
        var boundingBoxArea = horizontalDiameter > verticalDiameter
            ? horizontalDiameter * horizontalDiameter
            : verticalDiameter * verticalDiameter;
        var boundingCircleArea = boundingBoxArea * Math.PI / 4d;
        Assert.Multiple(() =>
        {
            Assert.That(Math.Abs(upperRadius - lowerRadius), Is.LessThanOrEqualTo(10));
            Assert.That(Math.Abs(rightRadius - leftRadius), Is.LessThanOrEqualTo(10));
            Assert.That(Math.Abs(verticalDiameter - horizontalDiameter), Is.LessThanOrEqualTo(10));
            Assert.That(areaCoveredByRectangles / (double)boundingBoxArea - Math.PI / 4d, Is.LessThanOrEqualTo(0.01));
            Assert.That(areaCoveredByRectangles / boundingCircleArea, Is.GreaterThanOrEqualTo(0.8));
        });
    }

    private int GetAreaCoveredByRectangles(List<Rectangle> layout)
    {
        return layout.Sum(rectangle => rectangle.Width * rectangle.Height);
    }

    private (int upper, int lower, int right, int left) GetRadii(List<Rectangle> layout)
    {
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        foreach (var rectangle in layout)
        {
            if (rectangle.Bottom > maxY) maxY = rectangle.Bottom;
            if (rectangle.Top < minY) minY = rectangle.Top;
            if (rectangle.Right > maxX) maxX = rectangle.Right;
            if (rectangle.Bottom < minX) minX = rectangle.Left;
        }
        var upperRadius = CENTER.Y - minY;
        var lowerRadius = maxY - CENTER.Y;
        var rightRadius = maxX - CENTER.X;
        var leftRadius = CENTER.X - minX;
        
        return (upperRadius, lowerRadius, rightRadius, leftRadius);
    }
}