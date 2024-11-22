using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Renderer;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class RendererTests
{
    private const string DefaultFileName = "image.png";
    private Renderer renderer;

    [SetUp]
    public void SetUp()
    {
        renderer = new Renderer(new SKSize(100, 100));
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(DefaultFileName))
            File.Delete(DefaultFileName);
        
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            SaveImage();
        }
    }

    private void SaveImage()
    {
        var filename = "tests/renderer_" + TestContext.CurrentContext.Test.ID + ".png";
        Directory.CreateDirectory("tests");
        var path = Path.Combine(Directory.GetCurrentDirectory(), filename);
        using var stream = new FileStream(path, FileMode.Create);
        var imageData = renderer.GetEncodedImage();
        imageData.SaveTo(stream);
        Console.WriteLine($"Attempted to save result to file {path}");
    }


    [Test]
    public void CreateImage_ShouldCreateImage()
    {
        var image = renderer.GetEncodedImage();
        image.Should().NotBeNull();
    }

    [TestCase(0, 200)]
    [TestCase(-1, 50)]
    public void CreateRectangles_ShouldThrowException_WhenRectanglesAreOutOfBounds(int topLeft, int bottomRight)
    {
        var action = () => renderer.DrawRectangles([new SKRect(topLeft, topLeft, bottomRight, bottomRight)]);
        action.Should().ThrowExactly<ArgumentException>();
    }

    [TestCase(0, 0, 0, 0)]
    [TestCase(50, 50, 0, 0)]
    [TestCase(50, 0, 50, 0)]
    public void CreateRectangles_ShouldThrowException_WhenRectanglesAreInvalid(int left, int top,
        int right, int bottom)
    {
        Assert.Throws<ArgumentException>(() =>
            renderer.DrawRectangles([new SKRect(left, top, right, bottom)]));
    }
}