using System.Drawing;
using FluentAssertions;
using TagCloudVisualization;

namespace TagCloudVisualizationTests;

public class VisualizationTests
{
    private string testOutputDir;
    
    [SetUp]
    public void Setup()
    {
        testOutputDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestOutput");
        CleanupTestDirectory();
        Directory.CreateDirectory(testOutputDir);
    }
    
    [TearDown]
    public void TearDown()
    {
        CleanupTestDirectory();
    }
    
    private void CleanupTestDirectory()
    {
        if (Directory.Exists(testOutputDir))
            Directory.Delete(testOutputDir, true);
    }
    
    [Test]
    public void CreateVisualization_CloudCenter_ShouldBeAtImageCenter()
    {
        var visualizer = new TagCloudVisualizer();
        var rectangles = CreateTestRectangles();
        var center = new Point(100, 100);
        var filePath = Path.Combine(testOutputDir, "centered_test.png");
        
        visualizer.CreateVisualization(rectangles, center, filePath);
        
        File.Exists(filePath).Should().BeTrue("Image file should be created");
        
        using var bitmap = new Bitmap(filePath);
        var imageCenter = new Point(bitmap.Width / 2, bitmap.Height / 2);
        
        bool foundRed = IsRedColorAtPoint(bitmap, imageCenter);
        
        foundRed.Should().BeTrue();
    }
    
    [Test]
    public void CreateVisualization_Image_ShouldBeSquare()
    {
        var visualizer = new TagCloudVisualizer();
        var rectangles = CreateTestRectangles();
        var center = new Point(100, 100);
        var filePath = Path.Combine(testOutputDir, "square_test.png");
        
        visualizer.CreateVisualization(rectangles, center, filePath);
        
        File.Exists(filePath).Should().BeTrue();
        
        using var bitmap = new Bitmap(filePath);
        
        bitmap.Width.Should().Be(bitmap.Height);
    }
    
    [Test]
    public void CreateVisualization_WithValidData_ShouldCreateImageFile()
    {
        var visualizer = new TagCloudVisualizer();
        var rectangles = CreateTestRectangles();
        var center = new Point(100, 100);
        var filePath = Path.Combine(testOutputDir, "valid_test.png");
        
        visualizer.CreateVisualization(rectangles, center, filePath);
        
        File.Exists(filePath).Should().BeTrue();
        
        var fileInfo = new FileInfo(filePath);
        fileInfo.Length.Should().BeGreaterThan(0);
        
        using var bitmap = new Bitmap(filePath);
        bitmap.Should().NotBeNull("File should be a valid image.");
        bitmap.Width.Should().BePositive("Image width should be positive.");
        bitmap.Height.Should().BePositive("Image height should be positive.");
    }
    
    
    private List<Rectangle> CreateTestRectangles()
    {
        return new List<Rectangle>
        {
            new Rectangle(0, 0, 50, 30),
            new Rectangle(60, 0, 40, 25),
            new Rectangle(0, 40, 35, 35),
            new Rectangle(60, 40, 45, 20),
            new Rectangle(120, 80, 30, 40)
        };
    }
    
    private bool IsRedColorAtPoint(Bitmap bitmap, Point point)
    {
        for (int dx = -2; dx <= 2; dx++)
        {
            for (int dy = -2; dy <= 2; dy++)
            {
                var x = Math.Clamp(point.X + dx, 0, bitmap.Width - 1);
                var y = Math.Clamp(point.Y + dy, 0, bitmap.Height - 1);
                
                var pixel = bitmap.GetPixel(x, y);
                
                if (pixel.R > 150 && pixel.R > pixel.G * 1.5 && pixel.R > pixel.B * 1.5)
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    [Test]
    public void CreateVisualization_WithEmptyRectangles_ShouldThrowArgumentException()
    {
        var visualizer = new TagCloudVisualizer();
        var rectangles = new List<Rectangle>();
        var center = new Point(100, 100);
        var filePath = Path.Combine(testOutputDir, "test.png");
        
        Action act = () => visualizer.CreateVisualization(rectangles, center, filePath);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Rectangles list cannot be empty");
    }
    
    [Test]
    public void CreateVisualization_WithNullRectangles_ShouldThrowArgumentNullException()
    {
        var visualizer = new TagCloudVisualizer();
        List<Rectangle> rectangles = null;
        var center = new Point(100, 100);
        var filePath = Path.Combine(testOutputDir, "test.png");
        
        Action act = () => visualizer.CreateVisualization(rectangles, center, filePath);
        
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("rectangles")
            .WithMessage("*rectangles*",
                "Null rectangles parameter should be rejected to prevent NullReferenceException.");
    }
    
    [Test]
    public void CreateVisualization_WithEmptyFilePath_ShouldThrowArgumentException()
    {
        var visualizer = new TagCloudVisualizer();
        var rectangles = CreateTestRectangles();
        var center = new Point(100, 100);
        
        Action act = () => visualizer.CreateVisualization(rectangles, center, "");

        act.Should().Throw<ArgumentException>()
            .WithMessage("File path cannot be empty");
    }
}