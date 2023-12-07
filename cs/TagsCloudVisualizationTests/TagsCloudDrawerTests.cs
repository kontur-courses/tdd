using System.Drawing;
using Moq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class TagsCloudDrawerTests
{
    private Mock<ICloudLayouter> cloudLayouter = null!;
    private TagsCloudDrawer drawer = null!;
    private Pen pen = null!;

    [SetUp]
    public void SetUp()
    {
        cloudLayouter = new Mock<ICloudLayouter>();
        pen = new Pen(Color.Red, 3);
        drawer = new TagsCloudDrawer(cloudLayouter.Object, pen, 3);
    }

    [TestCase("test", "<>\\")]
    [TestCase("test", "name|123")]
    [TestCase("test", null)]
    [TestCase("test", "")]
    [TestCase(null, "filename")]
    [TestCase("", "filename")]
    [TestCase("  ", "filename")]
    [TestCase(@"\:\", "filename")]
    public void SaveImage_ThrowsArgumentException_OnInvalidParameters(string dirPath, string filename)
    {
        Assert.Throws<ArgumentException>(() => TagsCloudDrawer.SaveImage(new Bitmap(1, 1), dirPath, filename));
    }

    [Test]
    public void DrawTagCloud_ThrowsArgumentException_WhenListOfRectanglesIsNull()
    {
        Assert.Throws<ArgumentException>(() => drawer.DrawTagCloud());
    }

    [TestCase(false, -2)]
    [TestCase(false, 0)]
    [TestCase(true, 2)]
    public void Constructor_ThrowsArgumentException_OnInvalidArguments(bool penIsNull, int scale)
    {
        Assert.Throws<ArgumentException>(() =>
            new TagsCloudDrawer(cloudLayouter.Object, (penIsNull ? null : pen)!, scale));
    }
}