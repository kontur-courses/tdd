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

    [TestCase("test", "<>\\")]
    [TestCase("test", "name|123")]
    [TestCase("test", null)]
    [TestCase("test", "")]
    [TestCase(null, "filename")]
    [TestCase("","filename")]
    [TestCase("  ","filename")]
    [TestCase(@"\:\","filename")]
    public void SaveImage_ThrowsArgumentException_OnInvalidParameters(string dirPath, string filename)
    {
        Assert.Throws<ArgumentException>(() => TagsCloudDrawer.SaveImage(new Bitmap(1, 1), dirPath, filename));
    }
}