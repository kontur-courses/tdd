using System.Drawing;
using System.Runtime.InteropServices;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Draw;
using TagsCloudVisualization.Extension;
using TagsCloudVisualization.RectangleGenerator;
using TagsCloudVisualization.Saver;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class ImageSaverTest
{
    private Point center;
    private CircularCloudLayouter layouter;
    private IEnumerable<Rectangle> rectangles;
    private RectangleDraftsman drawer;

    [SetUp]
    public void SetUp()
    {
        center = new Point(0, 0);
        layouter = new CircularCloudLayouter(center);
        rectangles = RectangleGenerator.GenerateRandomRectangles(10);
        layouter.PutRectangles(rectangles);
        drawer = new RectangleDraftsman(1500, 1500);
    }

    [TestCase(null)]
    public void CreateImage_OnInvalidParameters_ThrowsArgumentException(string filename)
    {
        drawer.CreateImage(layouter.Rectangles);
        var action = () => drawer.SaveImageToFile(filename);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase("12\\")]
    [TestCase("@#$\\")]
    public void CreateImage_OnInvalidParameters_ThrowsDirectoryNotFoundException(string filename)
    {
        drawer.CreateImage(layouter.Rectangles);
        var action = () => drawer.SaveImageToFile(filename);
        action.Should().Throw<DirectoryNotFoundException>();
    }

    [TestCase("abc|123")]
    [TestCase("123|abc")]
    [TestCase("123\n")]
    [TestCase("123\r")]
    [TestCase("\\")]
    [TestCase("")]
    public void CreateImage_OnInvalidParameters_ThrowsExternalException(string filename)
    {
        drawer.CreateImage(layouter.Rectangles);
        var action = () => drawer.SaveImageToFile(filename);
        action.Should().Throw<ExternalException>();
    }
}