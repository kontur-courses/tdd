using System.Drawing;
using NSubstitute;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class TagsCloudDrawerTests
{
    private TagsCloudDrawer drawer = null!;
    private Pen pen = null!;
    
    [SetUp]
    public void SetUp()
    {
        var layouter = Substitute.For<ICloudLayouter>();
        drawer = new TagsCloudDrawer(layouter);
        pen = new Pen(Color.Red, 1);
    }
    
    [TestCase(false, -2)]
    [TestCase(false, 0)]
    [TestCase(true, 2)]
    public void DrawRectangles_ThrowsArgumentException_OnInvalidArguments(bool penIsNull, int scale)
    {
        Assert.Throws<ArgumentException>(() => drawer.DrawRectangles((penIsNull ? null : pen)!, scale));
    }

    [TestCase("<>\\")]
    [TestCase("name|123")]
    [TestCase(null)]
    [TestCase("")]
    public void SaveImage_ThrowsArgumentException_OnInvalidFilename(string filename)
    {
        Assert.Throws<ArgumentException>(() => TagsCloudDrawer.SaveImage(new Bitmap(1, 1), filename));
    }
    
}