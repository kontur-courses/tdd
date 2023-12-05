using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class RectangleDrawerTests
{
    private Pen pen = null!;
    private RectangleDrawer drawer = null!;
    
    [SetUp]
    public void SetUp()
    {
        pen = new Pen(Color.Red, 5);
        drawer = new RectangleDrawer(pen, 5);
    }

    [Test]
    public void DrawRectangles_ThrowsArgumentException_WhenListOfRectanglesIsNull()
    {
        Assert.Throws<ArgumentException>(() => drawer.DrawRectangles(null!, new Rectangle(1,1,1,1)));
    }
    
    [TestCase(false, -2)]
    [TestCase(false, 0)]
    [TestCase(true, 2)]
    public void Constructor_ThrowsArgumentException_OnInvalidArguments(bool penIsNull, int scale)
    {
        Assert.Throws<ArgumentException>(() => new RectangleDrawer((penIsNull ? null : pen)!, scale));
    }
}