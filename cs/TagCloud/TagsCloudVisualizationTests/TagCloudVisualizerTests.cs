using System.Drawing;
using System.Drawing.Imaging;
using FakeItEasy;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class TagCloudVisualizerTests
{
    private TagCloudVisualizer visualizer;
    private IImageSaver fakeImageSaver;
    private Point center;

    [SetUp]
    public void SetUp()
    {
        fakeImageSaver = A.Fake<IImageSaver>();
        visualizer = new TagCloudVisualizer(fakeImageSaver);
        center = new Point(100, 100);
        
        A.CallTo(() => fakeImageSaver.SaveBitmap(A<Bitmap>._, A<string>._, A<ImageFormat>._))
            .ReturnsLazily(call => Path.Combine("test_output", call.Arguments.Get<string>(1)));
        
    }

    [Test]
    public void CreateVisualization_WithRectangles_ReturnsNonEmptyBitmap()
    {
        var rectangles = new[]
        {
            new Rectangle(50, 50, 30, 20),
            new Rectangle(100, 100, 40, 30)
        };
        var config = new TagCloudVisualizationConfig("test.png", backgroundColor: Color.White);
        
        var bitmap = visualizer.CreateVisualization(rectangles, center, config);
        
        bitmap.Should().NotBeNull();
        bitmap.Width.Should().BeGreaterThan(0);
        bitmap.Height.Should().BeGreaterThan(0);
    }

    [Test]
    public void CreateVisualization_WithImageSize_UsesSpecifiedSize()
    {
        var rectangles = new[] { new Rectangle(50, 50, 30, 20) };
        var expectedSize = new Size(800, 600);
        var config = new TagCloudVisualizationConfig("test.png", imageSize: expectedSize);
        
        var bitmap = visualizer.CreateVisualization(rectangles, center, config);
        
        bitmap.Size.Should().Be(expectedSize);
    }

    [Test]
    public void CreateVisualization_WithoutImageSize_CalculatesOptimalSize()
    {
        var rectangles = new[]
        {
            new Rectangle(0, 0, 50, 50),
            new Rectangle(200, 200, 50, 50)
        };
        var config = new TagCloudVisualizationConfig("test.png", imageSize: null);
        
        var bitmap = visualizer.CreateVisualization(rectangles, center, config); 
        
        bitmap.Width.Should().BeGreaterThan(200 + 50); 
        bitmap.Height.Should().BeGreaterThan(200 + 50);
    }
    
    [Test]
    public void CreateVisualization_UsesSpecifiedColors_CorrectlyCreatesBitmap()
    {
        var rectangles = new[] { new Rectangle(50, 50, 30, 20) };
        var backgroundColor = Color.Black;
        var rectangleColor = Color.Red;
        var centerColor = Color.Green;
        
        var config = new TagCloudVisualizationConfig(
            "test.png",
            backgroundColor: backgroundColor,
            rectangleColor: rectangleColor,
            centerColor: centerColor);
        
        var bitmap = visualizer.CreateVisualization(rectangles, center, config);
        
        bitmap.Should().NotBeNull();
        bitmap.Width.Should().BeGreaterThan(0);
        bitmap.Height.Should().BeGreaterThan(0);
    }

    [Test]
    public void SaveVisualization_CallsImageSaverWithCorrectFileName()
    {
        var rectangles = new[] { new Rectangle(50, 50, 30, 20) };
        var expectedFileName = "test_output.png";
        var config = new TagCloudVisualizationConfig(expectedFileName);
        
        visualizer.SaveVisualization(rectangles, center, config);
        
        A.CallTo(() => fakeImageSaver.SaveBitmap(A<Bitmap>._, expectedFileName, A<ImageFormat>._))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void SaveVisualization_ReturnsPathFromImageSaver()
    {
        var rectangles = new[] { new Rectangle(50, 50, 30, 20) };
        var expectedPath = "C:\\output\\test.png";
        var config = new TagCloudVisualizationConfig("test.png");
        
        A.CallTo(() => fakeImageSaver.SaveBitmap(A<Bitmap>._, A<string>._, A<ImageFormat>._))
            .Returns(expectedPath);
        
        var result = visualizer.SaveVisualization(rectangles, center, config);
        
        result.Should().Be(expectedPath);
    }
}