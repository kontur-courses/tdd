using System.Drawing;
using FluentAssertions;
using TagsCloudVizualization;

namespace TagsCloudVizualizationTests;

public class VisualizerTests
{
    private readonly string testDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestImages");

    [SetUp]
    public void Setup()
    {
        Directory.CreateDirectory(testDirectory);
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(testDirectory, recursive: true);
    }

    [Test]
    public void VisualizeRectangles_ShouldCreateBitmap()
    {
        var rectangles = new[] { new Rectangle(0, 0, 10, 10), new Rectangle(15, 15, 20, 20) };

        var bitmap = Visualizer.VisualizeRectangles(rectangles, 100, 100);
        
        bitmap.Should().NotBeNull();
    }

    [Test]
    public void SaveBitmap_ShouldSaveImageToDirectory()
    {
        var bitmap = new Bitmap(100, 100);
        
        Visualizer.SaveBitmap(bitmap, "test_image.png", testDirectory);

        var imagePath = Path.Combine(testDirectory, "test_image.png");
        File.Exists(imagePath).Should().BeTrue();
    }

    [Test]
    public void SaveBitmap_ShouldThrowException_WhenDirectoryNotExists()
    {
        var bitmap = new Bitmap(100, 100);
        var nonExistentDirectory = Path.Combine(testDirectory, "NonExistent");
        
        Assert.Throws<DirectoryNotFoundException>(() =>
            Visualizer.SaveBitmap(bitmap, "test_image.png", nonExistentDirectory));
    }
}